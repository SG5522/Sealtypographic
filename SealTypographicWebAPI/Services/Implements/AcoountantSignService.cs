using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using DJLib.Models;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Entities.TypographicModels;
using DBEntities.Entities.AccountantModels;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師簽印管理
    /// </summary>
    public class AcoountantSignService : IAccountantSignService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageService;
        private readonly IMapper mapper;   
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<AcoountantSignService> logger;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageService"></param>
        /// <param name="logger"></param>                
        public AcoountantSignService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService, ILogger<AcoountantSignService> logger)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.imageService = imageService;            
            this.logger = logger;
        }

        ///<inheritdoc />
        public AccountantSignGroupResponse GetCreateDates(int accountantId, int userId = 1)
        {
            logger.LogInformation("GetCreateDates input accountantId: {@accountantId} userId: {@userId}", accountantId, userId);

            AccountantSignGroupResponse accountantSignStartDates = new();

            try
            {
                IQueryable<AccountantSignGroupViewModel> AccountantSignGroupQuery = dbContext.AccountantSignGroups
                                                                                    .Where
                                                                                    (
                                                                                        accountantSignGroup => accountantSignGroup.Accountant.Id == accountantId
                                                                                        && accountantSignGroup.ReviewStatus <= ReviewStatus.Disabled
                                                                                        && accountantSignGroup.DeleteStatus == DeleteStatus.No
                                                                                    ).ProjectTo<AccountantSignGroupViewModel>(configurationProvider)
                                                                                    .OrderByDescending(accountantSignGroup => accountantSignGroup.GroupCreateDate);

                if (AccountantSignGroupQuery.Any())
                {
                    accountantSignStartDates.AccountantSignGroups = AccountantSignGroupQuery.ToList();
                    accountantSignStartDates.Success();
                }
                else
                {
                    accountantSignStartDates.DbNoData();
                }
                logger.LogInformation("GetCreateDates output {@Output}", accountantSignStartDates);
            }
            catch (Exception ex) 
            {
                accountantSignStartDates.Error();
                logger.LogError("GetCreateDates error {@Error}", ex.Message);
            }
            
            return accountantSignStartDates;
        }

        ///<inheritdoc />
        public AccountantSignViewModels GetSignViewModels(int accountantSignGroupId, bool isTransparent, int userId = 1)
        {
            logger.LogInformation("GetSignViewModels input accountantSignGroupId: {@accountantSignGroupId} isTransparent: {@isTransparent} userId: {@userId}"
                , accountantSignGroupId, isTransparent, userId);

            AccountantSignViewModels? accountantSignViewModels;

            try
            {
                accountantSignViewModels = dbContext.AccountantSignGroups
                                            .Include(x => x.TypographicResources)
                                            .ProjectTo<AccountantSignViewModels>(configurationProvider)
                                            .FirstOrDefault(x => x.AccountantSignGroupId == accountantSignGroupId);

                if (accountantSignViewModels != null)
                {
                    if (isTransparent)
                    {
                        foreach (AccountantSignViewModel accountantSignViewModel in accountantSignViewModels.SignViewModels)
                        {
                            ImageInfo imageInfo = ImageInfo.FromImageBase64(accountantSignViewModel.ImageBase64);
                            accountantSignViewModel.ImageBase64 = imageInfo.TransparentToImageBase64();
                        }
                    }
                    accountantSignViewModels.Success();
                }
                else
                {
                    accountantSignViewModels = new();
                    accountantSignViewModels.AccountantSignNoData();
                }
                logger.LogInformation("GetSignViewModels output {@Output}", accountantSignViewModels);
            }
            catch (Exception ex)
            {
                accountantSignViewModels = new();
                accountantSignViewModels.Error();
                logger.LogError("GetSignViewModels error {@Error}", ex.Message);
            }

            return accountantSignViewModels;
        }

        ///<inheritdoc />
        public async Task<ResponseViewModel> New(AccountantSignForms accountantSignForms, int userId = 1)
        {
            logger.LogInformation("New input {@accountantSignForms} userId: {@userId}", accountantSignForms, userId);

            ResponseViewModel response = new();            

            try
            {
                //確認是否有該會計師的資料
                Accountant? accountantQuery = dbContext.Accountants.Include(accountant => accountant.AccountantSignGroups)
                                                                    .FirstOrDefault
                                                                    (
                                                                        accountant => accountant.Id == accountantSignForms.AccountantId
                                                                        && accountant.DeleteStatus == DeleteStatus.No
                                                                    );

                if (accountantQuery != null)
                {
                    AccountantSignGroup accountantSignGroup = new();
                    //之後調整無需轉型
                    ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithSeal(accountantQuery.Code, SealType.Accountant);

                    BaseInputSignGroupJournal(accountantSignGroup, true, userId);
                    //新增簽印資料(圖檔與DB資源)         
                    accountantSignGroup.TypographicResources = await NewTypographyResource(accountantSignForms.SignForms, imageBase64Info, userId);

                    accountantQuery.AccountantSignGroups.Add(accountantSignGroup);
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.AccountantNoData();
                }
                logger.LogInformation("New output {@Output}", response);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogError("New error {@Error}", ex.Message);
            }
            
            return response;
        }

        ///<inheritdoc />
        public async Task<List<ResponseViewModel>> Update(AccountantSignUpdate accountantSignUpdate, int userId = 1)
        {
            logger.LogInformation("Update input {@Input} userId: {@userId}", accountantSignUpdate, userId);

            List<ResponseViewModel> responseViewModels = new();                        

            try
            {
                AccountantSignGroup? accountantSignGroupQuery = dbContext.AccountantSignGroups
                                                                .Include(accountantSignGroup => accountantSignGroup.Accountant)
                                                                .Include(accountantSignGroup => accountantSignGroup.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No))
                                                                .FirstOrDefault
                                                                (
                                                                    accountantSignGroup => accountantSignGroup.Id == accountantSignUpdate.AccountantSignGroupId
                                                                );

                if (accountantSignGroupQuery != null)
                {
                    //舊的會計師簽印群組停用
                    BaseInputSignGroupJournal(accountantSignGroupQuery, false, userId);

                    Accountant? accountant = dbContext.Accountants.Include(accountant => accountant.AccountantSignGroups)
                                            .FirstOrDefault(accountant => accountant.Id == accountantSignGroupQuery.Accountant.Id);

                    if (accountant != null)
                    {
                        //宣告新的簽印
                        AccountantSignGroup accountantSignGroup = new()
                        {
                            TypographicResources = new List<TypographicResource>()
                        };

                        //之後拔除轉型調整
                        ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithSeal(accountant.Code, SealType.Accountant);

                        //修改(更新ID移入DeleteAccountantSignIds，更新的簽印移入新增CreateAccountantSigns)
                        foreach (AccountantSignUpdateForm accountantSignFormUpdate in accountantSignUpdate.UpdateAccountantSigns)
                        {
                            TypographicResource? updateSignQuery = accountantSignGroupQuery.TypographicResources.FirstOrDefault(x => x.Id == accountantSignFormUpdate.Id);

                            if (updateSignQuery != null)
                            {
                                AccountantSign accountantSign = new()
                                {
                                    ImageBase64 = accountantSignFormUpdate.ImageBase64,
                                    SealMappingConfigId = SealMappingConfigUtil.GetAccountantSignType(updateSignQuery.SubSealType),
                                };

                                //更新ID丟入DeleteAccountantSignIds
                                accountantSignUpdate.DeleteAccountantSignIds.Add(accountantSignFormUpdate.Id);
                                //更新的簽印移入新增
                                accountantSignUpdate.CreateAccountantSigns.Add(accountantSign);
                            }
                            else
                            {
                                ResponseViewModel response = new();
                                response.UpdateAccountantSignNoData();
                                response.ErrorItem = $"Update AccountantSignid:{accountantSignFormUpdate.Id}";
                                responseViewModels.Add(response);
                            }
                        }

                        //刪除及更新簽印排除
                        IQueryable<TypographicResource>? deleteSignQuery = accountantSignGroupQuery.TypographicResources.Where
                                                                            (
                                                                                x => !accountantSignUpdate.DeleteAccountantSignIds.Contains(x.Id)
                                                                            ).AsQueryable();

                        //複製簽印不含刪除與更新的
                        foreach (TypographicResource copyTypographyResource in deleteSignQuery)
                        {
                            TypographicResource typographyResource = mapper.Map<TypographicResource>(copyTypographyResource);
                            accountantSignGroup.TypographicResources.Add(typographyResource);
                        }
                        BaseInputSignGroupJournal(accountantSignGroup, true, userId);

                        //新增簽印資料(圖檔與DB資源)         
                        accountantSignGroup.TypographicResources = await NewTypographyResource(accountantSignUpdate.CreateAccountantSigns, imageBase64Info, userId);

                        //沒有任何回傳訊息(錯誤訊息)就更新資料庫
                        if (!responseViewModels.Any())
                        {
                            ResponseViewModel response = new();
                            accountant.AccountantSignGroups.Add(accountantSignGroup);
                            dbContext.SaveChanges();
                            response.Success();
                            responseViewModels.Add(response);
                        }
                    }
                    else
                    {
                        ResponseViewModel response = new();
                        response.AccountantNoData();
                        responseViewModels.Add(response);
                    }
                }
                else
                {
                    ResponseViewModel response = new();
                    response.AccountantSignNoData();
                    responseViewModels.Add(response);
                }
                logger.LogInformation("Update output {@Output}", responseViewModels);
            }
            catch (Exception ex)
            {
                ResponseViewModel response = new();
                response.Error();
                responseViewModels.Add(response);
                logger.LogInformation("Update error {@Error}", ex.Message);
            }
                        
            return responseViewModels;
        }

        ///<inheritdoc />    
        public ResponseViewModel ChangeReviewStatus(int accountantSignGroupId, ReviewStatus reviewStatus, int userId = 1)
        {
            logger.LogInformation("ChangeReviewStatus input accountantSignGroupId: {@accountantSignGroupId} reviewStatus: {@reviewStatus} userId: {@userId}"
                , accountantSignGroupId, reviewStatus, userId);

            ResponseViewModel response = new();            
            
            try
            {
                AccountantSignGroup? accountantSignGroupQuery = dbContext.AccountantSignGroups.Find(accountantSignGroupId);

                if (accountantSignGroupQuery != null)
                {
                    accountantSignGroupQuery.ReviewStatus = reviewStatus;
                    accountantSignGroupQuery.UpdateUserId = userId;
                    accountantSignGroupQuery.UpdateDate = DateTime.Now;
                    if (reviewStatus == ReviewStatus.Invalid)
                    {
                        accountantSignGroupQuery.DeleteStatus = DeleteStatus.Yes;
                    }
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.UpdateAccountantSignNoData();
                }
                logger.LogInformation("ChangeReviewStatus output {@Ouput}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();                
                logger.LogError("ChangeReviewStatus dbError {@Dberror}", ex.Message);
            }
            catch (Exception ex) 
            {
                response.Error();                
                logger.LogError("ChangeReviewStatus error {@Error}",ex.Message);
            }

            return response;
        }

        /// <summary>
        /// 會計師簽印建立日期歷程基本資料輸入
        /// </summary>
        /// <param name="accountantSignGroup">會計師簽印建立日期歷程</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputSignGroupJournal(AccountantSignGroup accountantSignGroup, bool isCreate, int userId = 1)
        {
            if (isCreate)
            {
                accountantSignGroup.CreateUserId = userId;
                accountantSignGroup.CreateDate = DateTime.Now;
                accountantSignGroup.DeleteStatus = DeleteStatus.No;           
                accountantSignGroup.ReviewStatus = ReviewStatus.Draft;
            }
            else
            {
                //如果變更的簽印組狀態是通過將結束日期更新為現在
                if (accountantSignGroup.ReviewStatus == ReviewStatus.Approval)
                {
                    accountantSignGroup.EndDate = DateTime.Now;
                }
                accountantSignGroup.UpdateUserId = userId;
                accountantSignGroup.UpdateDate = DateTime.Now;
                accountantSignGroup.ReviewStatus = ReviewStatus.Disabled;
            }
        }                

        /// <summary>
        /// 新增印鑑、簽印、圖片資料
        /// </summary>
        /// <param name="formSeals">輸入</param>        
        /// <param name="imageBase64Info">圖檔資訊</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        private async Task<List<TypographicResource>> NewTypographyResource(List<AccountantSign> formSeals, ImageBase64Info imageBase64Info, int userId = 1)
        {
            List<TypographicResource> typographyResources = new();
            foreach (AccountantSign accountantSign in formSeals)
            {
                TypographicResource typographyResource = new()
                {
                    SealType = SealType.Accountant,
                    //輸入model之後要修正為新的db
                    SubSealType = SealMappingConfigUtil.GetSubSealTypeWithAccountant((AccountantSignType)accountantSign.SealMappingConfigId),
                };
                //ImageBase64轉圖檔並存到指定資料夾
                imageBase64Info.ImageBase64 = accountantSign.ImageBase64;
                typographyResource.ImageFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                typographyResource.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, true);

                TypographicResourceUtil.BaseInputTypographyResource(typographyResource, true, userId);
                typographyResources.Add(typographyResource);
            }
            return typographyResources;
        }
    }
}

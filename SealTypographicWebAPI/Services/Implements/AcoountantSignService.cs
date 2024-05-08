using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Entities.TypographicModels;
using DBEntities.Entities.AccountantModels;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Models.LogReport.AccountantSignLog;
using CommonLib.Enums;
using DBEntities.Extensions;
using DBEntities.Utils;

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
        private readonly ILogReportService logReportService;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageService"></param>
        /// <param name="logger"></param>
        /// <param name="logReportService"></param>                
        public AcoountantSignService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService, ILogger<AcoountantSignService> logger, ILogReportService logReportService)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.imageService = imageService;            
            this.logger = logger;
            this.logReportService = logReportService;
        }

        ///<inheritdoc />
        public async Task<AccountantSignGroupResponse> GetCreateDates(int accountantId, int userId = 1)
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
                    accountantSignStartDates.AccountantSignGroups = await AccountantSignGroupQuery.ToListAsync();
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
        public async Task<AccountantSignViewModels> GetSignViewModels(int accountantSignGroupId, bool isTransparent, UserInfo userInfo)
        {
            logger.LogInformation("GetSignViewModels input accountantSignGroupId: {@accountantSignGroupId} isTransparent: {@isTransparent} userId: {@userId}"
                , accountantSignGroupId, isTransparent, userInfo.UserId);

            AccountantSignViewModels? accountantSignViewModels;

            try
            {
                accountantSignViewModels = await dbContext.AccountantSignGroups
                                            .Include(x => x.Accountant)
                                            .Include(x => x.TypographicResources)
                                            .ProjectTo<AccountantSignViewModels>(configurationProvider)
                                            .FirstOrDefaultAsync(x => x.AccountantSignGroupId == accountantSignGroupId);

                if (accountantSignViewModels != null)
                {
                    if (isTransparent)
                    {
                        foreach (AccountantSignViewModel accountantSignViewModel in accountantSignViewModels.SignViewModels)
                        {                            
                            accountantSignViewModel.ImageBase64 = ImageTransparentUtil.ToDataUrlFromDataUrl(accountantSignViewModel.ImageBase64);
                        }
                    }
                    accountantSignViewModels.Success();
                    //操作紀錄(查詢)存檔
                    await logReportService.SaveOperationLog(
                                                                mapper.Map<OperationLogSave>(accountantSignViewModels),
                                                                userInfo.UserName,
                                                                $"{userInfo.FirstName}{userInfo.LastName}"
                                                            );
                }
                else
                {
                    accountantSignViewModels = new();
                    accountantSignViewModels!.AccountantSignNoData();
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
        public async Task<ResponseViewModel> New(AccountantSignForms accountantSignForms, UserInfo userInfo)
        {
            logger.LogInformation("New input {@accountantSignForms} userId: {@userId}", accountantSignForms, userInfo.UserId);

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
                    ImageSaveInfo imageBase64Info = imageService.SetImageBase64InfoWithSeal(accountantQuery.Code, SealType.Accountant);
                    //建立此組簽印的審核類型與日期與建立日期
                    InputUtil.SetDraftWithCreate(accountantSignGroup, userInfo.UserId);

                    //新增簽印資料(圖檔與DB資源)         
                    accountantSignGroup.TypographicResources = await NewTypographyResource(accountantSignForms.SignForms, imageBase64Info, userInfo.UserId);

                    accountantQuery.AccountantSignGroups.Add(accountantSignGroup);
                    dbContext.SaveChanges();
                    response.Success();
                    //異動紀錄存檔(新增)
                    await logReportService.SaveAccountantSignEventLog(
                                                                        mapper.Map<AccountantSignEventLogSave>(accountantSignGroup), OperateType.Create,
                                                                        userInfo.UserName,
                                                                        $"{userInfo.FirstName}{userInfo.LastName}"
                                                                    );
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
        public async Task<List<ResponseViewModel>> Update(AccountantSignUpdate accountantSignUpdate, UserInfo userInfo)
        {
            logger.LogInformation("Update input {@Input} userId: {@userId}", accountantSignUpdate, userInfo.UserId);

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
                    InputUtil.SetDisabled(accountantSignGroupQuery);

                    Accountant? accountant = dbContext.Accountants.Include(accountant => accountant.AccountantSignGroups)
                                            .FirstOrDefault(accountant => accountant.Id == accountantSignGroupQuery.Accountant.Id);

                    if (accountant != null)
                    {
                        //宣告新的簽印
                        AccountantSignGroup accountantSignGroup = new()
                        {
                            TypographicResources = new List<TypographicResource>()
                        };
                        
                        ImageSaveInfo imageSaveInfo = imageService.SetImageBase64InfoWithSeal(accountant.Code, SealType.Accountant);

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
                        //審核狀態設定為草稿
                        InputUtil.SetDraftWithCreate(accountantSignGroup, userInfo.UserId);

                        //新增簽印資料(圖檔與DB資源)         
                        accountantSignGroup.TypographicResources = await NewTypographyResource(accountantSignUpdate.CreateAccountantSigns, imageSaveInfo, userInfo.UserId);

                        //沒有任何回傳訊息(錯誤訊息)就更新資料庫
                        if (!responseViewModels.Any())
                        {
                            ResponseViewModel response = new();
                            accountant.AccountantSignGroups.Add(accountantSignGroup);
                            dbContext.SaveChanges();
                            response.Success();
                            //異動紀錄存檔(修改)
                            await logReportService.SaveAccountantSignEventLog(
                                                                                mapper.Map<AccountantSignEventLogSave>(accountantSignGroup), OperateType.Modify,
                                                                                userInfo.UserName,
                                                                                $"{userInfo.FirstName}{userInfo.LastName}"
                                                                            );
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
        public async Task<ResponseViewModel> ChangeReviewStatus(int accountantSignGroupId, ReviewStatus reviewStatus, int userId = 1)
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
                    await dbContext.SaveChangesAsync();
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
        /// 新增印鑑、簽印、圖片資料
        /// </summary>
        /// <param name="formSeals">輸入</param>        
        /// <param name="imageSaveInfo">圖檔資訊</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        private async Task<List<TypographicResource>> NewTypographyResource(List<AccountantSign> formSeals, ImageSaveInfo imageSaveInfo, int userId = 1)
        {
            List<TypographicResource> typographyResources = new();
            foreach (AccountantSign accountantSign in formSeals)
            {
                TypographicResource typographyResource = new()
                {
                    SealType = SealType.Accountant,
                    //輸入model之後要修正為新的db
                    SubSealType = SealMappingConfigUtil.GetSubSealTypeWithAccountant(accountantSign.SealMappingConfigId),
                };
                //ImageBase64轉圖檔並存到指定資料夾
                imageSaveInfo.ImageBase64 = accountantSign.ImageBase64;
                typographyResource.ImageFullPath = await imageService.GetSavedImageFilePath(imageSaveInfo);
                typographyResource.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageSaveInfo, true);
                InputUtil.Set(typographyResource, userId, true);                
                typographyResources.Add(typographyResource);
            }
            return typographyResources;
        }
    }
}

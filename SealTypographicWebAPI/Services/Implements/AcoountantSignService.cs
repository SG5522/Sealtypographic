using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Utils;
using DBEntities;
using DBEntities.Consts;
using DJLib.Models;
using SealTypographicWebAPI.Models.Customer;

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
        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageService"></param>        
        public AcoountantSignService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
            this.imageService = imageService;            
        }

        /// <summary>
        /// 取得會計師簽印建立日期列表
        /// </summary>
        /// <param name="accountantId">會計師Id</param>
        /// <returns></returns>
        public AccountantSignGroupResponse GetCreateDates(int accountantId)
        {
            AccountantSignGroupResponse accountantSignStartDates = new()
            {
                AccountantSignGroups = dbContext.AccountantSignGroups
                                                       .Where
                                                       (
                                                            accountantSignGroup => accountantSignGroup.Accountant.Id == accountantId
                                                            && accountantSignGroup.ReviewStatus <= ReviewStatus.Disabled
                                                            && accountantSignGroup.DeleteStatus == DeleteStatus.No
                                                       )
                                                       .Select(accountantSignGroup => new AccountantSignGroupViewModel()
                                                       {
                                                           Id = accountantSignGroup.Id,
                                                           GroupCreateDate = accountantSignGroup.CreateDate,
                                                           //之後調整ReviewStatus不需轉型
                                                           ReviewStatus = (ReviewStatus)accountantSignGroup.ReviewStatus
                                                       })                                                       
                                                       .OrderByDescending(accountantSignGroup => accountantSignGroup.GroupCreateDate)                                                       
                                                       .ToList()
            };
            accountantSignStartDates.Success();

            return accountantSignStartDates;
        }

        /// <summary>
        /// 取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignGroupId"></param>
        /// <param name="isTransparent">是否白底透明化</param>
        /// <returns></returns>
        public AccountantSignViewModels GetSignViewModels(int accountantSignGroupId, bool isTransparent)
        {
            AccountantSignViewModels? accountantSignViewModels = mapper.ProjectTo<AccountantSignViewModels>
                                                                (
                                                                    dbContext.AccountantSignGroups
                                                                    .Include(x => x.TypographicResources)
                                                                ).FirstOrDefault(x => x.AccountantSignGroupId == accountantSignGroupId);

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

            return accountantSignViewModels;
        }

        /// <summary>
        /// 新增會計師簽印組
        /// </summary>
        /// <param name="accountantSignForms">會計師簽印組</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(AccountantSignForms accountantSignForms)
        {
            ResponseViewModel response = new();
            List<TypographicResource> typographyResources = new();
            int userId = 1; //從帳號驗證取得Id

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
                await NewTypographyResource(accountantSignForms.SignForms, typographyResources, imageBase64Info, userId);

                accountantSignGroup.TypographicResources = typographyResources;
                accountantQuery.AccountantSignGroups.Add(accountantSignGroup);
                dbContext.SaveChanges();
                response.Success();
            }                   
            else
            {
                response.AccountantNoData();
            }
            return response;
        }
        /// <summary>
        /// 異動會計師簽印
        /// </summary>
        /// <param name="accountantSignUpdate">需要異動會計師簽印資料</param>
        /// <returns></returns>
        public async Task<List<ResponseViewModel>> Update(AccountantSignUpdate accountantSignUpdate)
        {
            List<ResponseViewModel> responseViewModels = new();            
            int userId = 1;//之後會從帳號驗證中取得userid            

            AccountantSignGroup? accountantSignGroupQuery = dbContext.AccountantSignGroups
                                                                    .Include(accountantSignGroup => accountantSignGroup.Accountant)
                                                                    .Include(accountantSignGroup => accountantSignGroup.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No))
                                                                    .FirstOrDefault
                                                                    (
                                                                        accountantSignGroupJournal => accountantSignGroupJournal.Id == accountantSignUpdate.AccountantSignGroupId                                                                                        
                                                                    );
            

            if (accountantSignGroupQuery != null)
            {
                //舊的會計師簽印群組停用
                BaseInputSignGroupJournal(accountantSignGroupQuery, false, userId);                

                Accountant? accountant = dbContext.Accountants.Include(accountant => accountant.AccountantSignGroups)
                                        .FirstOrDefault(accountant => accountant.Id == accountantSignGroupQuery.Accountant.Id);

                if(accountant != null)
                {
                    //宣告新的簽印
                    AccountantSignGroup accountantSignGroup = new()
                    {
                        TypographicResources = new()
                    };

                    //之後拔除轉型調整
                    ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithSeal(accountant.Code, SealType.Accountant);

                    //修改(更新ID移入DeleteAccountantSignIds，更新的簽印移入新增CreateAccountantSigns，之後下一階段調整輸入時要拔掉此項)
                    foreach (AccountantSignUpdateForm accountantSignFormUpdate in accountantSignUpdate.UpdateAccountantSigns)
                    {
                        TypographicResource? updateSignQuery = accountantSignGroupQuery.TypographicResources.FirstOrDefault(x => x.Id == accountantSignFormUpdate.Id);

                        if (updateSignQuery != null)
                        {
                            AccountantSign accountantSign = new()
                            {
                                ImageBase64 = accountantSignFormUpdate.ImageBase64,
                                SealMappingConfigId = (AccountantSignType)SealMappingConfigUtil.GetAccountantSignType(updateSignQuery.SubSealType),
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

                    //新增
                    await NewTypographyResource(accountantSignUpdate.CreateAccountantSigns, accountantSignGroup.TypographicResources, imageBase64Info, userId);                    

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
                }
            }
            else
            {
                ResponseViewModel response = new();
                response.AccountantSignNoData();
            }
            
            return responseViewModels;
        }

        ///<inheritdoc />
        public ResponseViewModel Pending(int accountantSignGroupId)
        {            
            ResponseViewModel response = ChangeReviewStatus(accountantSignGroupId, ReviewStatus.Pending);
            return response;
        }

        ///<inheritdoc />           
        public ResponseViewModel Invalid(int accountantSignGroupId)
        {
            ResponseViewModel response = ChangeReviewStatus(accountantSignGroupId, ReviewStatus.Invalid);
            return response;
        }

        ///<inheritdoc />     
        public ResponseViewModel CancelReview(int accountantSignGroupId)
        {
            ResponseViewModel response = ChangeReviewStatus(accountantSignGroupId, ReviewStatus.Draft);
            return response;
        }

        /// <summary>
        /// 會計師簽印建立日期歷程基本資料輸入
        /// </summary>
        /// <param name="accountantSignGroup">會計師簽印建立日期歷程</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputSignGroupJournal(AccountantSignGroup accountantSignGroup, bool isCreate, int userId)
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
        /// 會計師印鑑待審狀態變更。
        /// </summary>
        /// <param name="accountantSignGroupId"></param>
        /// <param name="reviewStatus">審查狀態</param>        
        private ResponseViewModel ChangeReviewStatus(int accountantSignGroupId, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userid = 0; //從帳號驗證取得Id
            AccountantSignGroup? accountantSignGroupQuery = dbContext.AccountantSignGroups.Find(accountantSignGroupId);

            if(accountantSignGroupQuery != null)
            {
                accountantSignGroupQuery.ReviewStatus = reviewStatus;
                accountantSignGroupQuery.UpdateUserId = userid;
                accountantSignGroupQuery.UpdateDate = DateTime.Now;
                if(reviewStatus == ReviewStatus.Invalid)
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
            return response;
        }

        /// <summary>
        /// 新增印鑑、簽印、圖片資料
        /// </summary>
        /// <param name="formSeals">輸入</param>
        /// <param name="typographyResources">要輸入資料庫的資源</param>
        /// <param name="imageBase64Info">圖檔資訊</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        private async Task NewTypographyResource(List<AccountantSign> formSeals, List<TypographicResource> typographyResources, ImageBase64Info imageBase64Info, int userId)
        {
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
        }
    }
}

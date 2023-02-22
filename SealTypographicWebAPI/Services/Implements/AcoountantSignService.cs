using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Utils;
using System.Linq;

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
                AccountantSignGroups = dbContext.AccountantSignGroupJournals
                                                       .Where
                                                       (
                                                            accountantSignGroupJournal => accountantSignGroupJournal.Accountant.Id == accountantId
                                                            && accountantSignGroupJournal.ReviewStatus <= ReviewStatus.Disabled
                                                            && accountantSignGroupJournal.DeleteStatus == DeleteStatus.No
                                                       )
                                                       .Select(accountantSignGroupJournal => new AccountantSignGroupViewModel()
                                                       {
                                                           Id = accountantSignGroupJournal.Id,
                                                           GroupCreateDate = accountantSignGroupJournal.CreateDate,
                                                           ReviewStatus = accountantSignGroupJournal.ReviewStatus
                                                       })                                                       
                                                       .OrderByDescending(accountantSignGroup => accountantSignGroup.GroupCreateDate)                                                       
                                                       .ToList()
            };

            if (accountantSignStartDates.AccountantSignGroups.Any())
            {                                                
                accountantSignStartDates.Success();
            }
            else
            {
                accountantSignStartDates.AccountantSignNoData();
            }
            return accountantSignStartDates;
        }

        /// <summary>
        /// 取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignGroupId"></param>
        /// <returns></returns>
        public AccountantSignViewModels GetSignViewModels(int accountantSignGroupId)
        {
            AccountantSignViewModels signViewModels = new();

            AccountantSignGroupJournal? accountantSignGroupJournalQuery = dbContext.AccountantSignGroupJournals
                                                                .Include(accountantSignGroupJournal => accountantSignGroupJournal.AccountantSignJournals)
                                                                .FirstOrDefault
                                                                (
                                                                    accountantSignGroupJournal => accountantSignGroupJournal.Id == accountantSignGroupId
                                                                );

            if (accountantSignGroupJournalQuery != null)
            {
                List<AccountantSignJournal> accountantSigns = accountantSignGroupJournalQuery.AccountantSignJournals
                                                            .Where(x => x.DeleteStatus == DeleteStatus.No)
                                                            .OrderBy(x => x.ConfigType)                                                            
                                                            .ToList();

                foreach (AccountantSignJournal accountantSignJournal in accountantSigns)
                {
                    AccountantSignViewModel accountantSignViewModel = mapper.Map<AccountantSignViewModel>(accountantSignJournal);
                    accountantSignViewModel.ImageBase64 = imageService.GetPathToBase64(accountantSignJournal.ImageFullPath); //資料庫取得圖檔路徑轉BASE64                   
                    
                    accountantSignViewModel.SealMappingConfigId = accountantSignJournal.ConfigType;
                    signViewModels.SignViewModels.Add(accountantSignViewModel);
                }
                signViewModels.AccountantSignGroupId = accountantSignGroupId;
                signViewModels.GroupCreateDate = accountantSignGroupJournalQuery.CreateDate;
                signViewModels.ReviewStatus = accountantSignGroupJournalQuery.ReviewStatus;                
                signViewModels.Success();
            }
            else
            {
                signViewModels.AccountantSignNoData();
            }

            return signViewModels;
        }

        /// <summary>
        /// 新增會計師簽印組
        /// </summary>
        /// <param name="accountantSignForms">會計師簽印組</param>
        /// <returns></returns>
        public ResponseViewModel New(AccountantSignForms accountantSignForms)
        {
            ResponseViewModel response = new();
            List<AccountantSignJournal> accountantSignJournals = new();
            int userId = 0; //從帳號驗證取得Id

            //確認是否有一組草稿或待審的會計師簽印
            Accountant? accountantQuery = dbContext.Accountants.Include(accountant => accountant.AccountantSignGroupJournals)
                                .FirstOrDefault(accountant => accountant.Id == accountantSignForms.AccountantId);

            if (accountantQuery != null)
            {                
                AccountantSignGroupJournal accountantSignCreateDateJournal = new();
                ImageBase64Info imageBase64Info = new()
                {
                    Code = accountantQuery.Code,                    
                    SealType = SealType.Accountant
                };

                BaseInputSignGroupJournal(accountantSignCreateDateJournal, true, userId);                
                
                foreach (AccountantSign accountantSign in accountantSignForms.SignForms)
                {
                    AccountantSignJournal accountantSignJournal = new()
                    {
                        ConfigType = accountantSign.SealMappingConfigId
                    };
                                        
                    imageBase64Info.ImageBase64 = accountantSign.ImageBase64;
                    accountantSignJournal.ImageFullPath = imageService.GetImageBase64FullPath(imageBase64Info, false);
                    accountantSignJournal.ThumbnailFullPath = imageService.GetImageBase64FullPath(imageBase64Info, true);
                    BaseInputAccountantSignJournal(accountantSignJournal, true, userId);
                    accountantSignJournals.Add(accountantSignJournal);
                    
                }

                accountantSignCreateDateJournal.AccountantSignJournals = accountantSignJournals;
                accountantQuery.AccountantSignGroupJournals.Add(accountantSignCreateDateJournal);
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
        public List<ResponseViewModel> Update(AccountantSignUpdate accountantSignUpdate)
        {
            List<ResponseViewModel> responseViewModels = new();            
            int userId = 0;//之後會從帳號驗證中取得userid            

            AccountantSignGroupJournal? accountantSignGroupJournalQuery = dbContext.AccountantSignGroupJournals
                                                                        .Include(accountantSignGroupJournal => accountantSignGroupJournal.Accountant)
                                                                        .Include(accountantSignGroupJournal => accountantSignGroupJournal.AccountantSignJournals.Where(x => x.DeleteStatus == DeleteStatus.No))
                                                                        .FirstOrDefault
                                                                        (
                                                                            accountantSignGroupJournal => accountantSignGroupJournal.Id == accountantSignUpdate.AccountantSignGroupId                                                                                        
                                                                        );
            

            if (accountantSignGroupJournalQuery != null)
            {
                //舊的會計師簽印群組停用
                BaseInputSignGroupJournal(accountantSignGroupJournalQuery, false, userId);                

                Accountant? accountant = dbContext.Accountants.Include(accountant => accountant.AccountantSignGroupJournals)
                                .FirstOrDefault(accountant => accountant.Id == accountantSignGroupJournalQuery.Accountant.Id);

                if(accountant != null)
                {
                    //複製簽印
                    AccountantSignGroupJournal accountantSignGroup = new()
                    {
                        AccountantSignJournals = accountantSignGroupJournalQuery.AccountantSignJournals.ToList()
                    };
                    BaseInputSignGroupJournal(accountantSignGroup, true, userId);

                    ImageBase64Info imageBase64Info = new()
                    {
                        Code = accountant.Code,
                        SealType = SealType.Accountant,
                    };

                    //修改(更新ID移入DeleteAccountantSignIds，更新的簽印移入新增CreateAccountantSigns，之後下一階段調整輸入時要拔掉此項)
                    foreach (AccountantSignUpdateForm accountantSignFormUpdate in accountantSignUpdate.UpdateAccountantSigns)
                    {
                        AccountantSignJournal? updateSignQuery = accountantSignGroupJournalQuery.AccountantSignJournals.FirstOrDefault(x => x.Id == accountantSignFormUpdate.Id);

                        if (updateSignQuery != null)
                        {
                            AccountantSign accountantSign = new()
                            {
                                ImageBase64 = accountantSignFormUpdate.ImageBase64,
                                SealMappingConfigId = updateSignQuery.ConfigType
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

                    //更新及異動的印鑑去除
                    accountantSignGroup.AccountantSignJournals = accountantSignGroup.AccountantSignJournals
                                                            .Where(x => !accountantSignUpdate.DeleteAccountantSignIds.Contains(x.Id)).ToList();

                    foreach (AccountantSignJournal accountantSign in accountantSignGroup.AccountantSignJournals)
                    {
                        accountantSign.Id = 0;
                    }

                    //新增
                    foreach (AccountantSign createAccountantSign in accountantSignUpdate.CreateAccountantSigns)
                    {
                        AccountantSignCheck accountantSignCheck = new()
                        {
                            AccountantSignCreateDateJournalId = accountantSignGroupJournalQuery.Id,
                            SealMappingConfigId = createAccountantSign.SealMappingConfigId,
                        };

                        if (!CheckAccountSignRepeat(accountantSignCheck, accountantSignUpdate.DeleteAccountantSignIds))
                        {
                            AccountantSignJournal accountantSignJournal = new()
                            {
                                ConfigType = createAccountantSign.SealMappingConfigId
                            };

                            //ImageBase64轉圖檔並存到指定資料夾
                            imageBase64Info.ImageBase64 = createAccountantSign.ImageBase64;
                            accountantSignJournal.ImageFullPath = imageService.GetImageBase64FullPath(imageBase64Info, false);
                            accountantSignJournal.ThumbnailFullPath = imageService.GetImageBase64FullPath(imageBase64Info, true);
                            BaseInputAccountantSignJournal(accountantSignJournal, true, userId);
                            accountantSignGroup.AccountantSignJournals.Add(accountantSignJournal);
                        }
                        else
                        {
                            ResponseViewModel response = new();
                            response.CreateAccountantSignRepeat();
                            response.ErrorItem = $"New SealMappingConfigId:{createAccountantSign.SealMappingConfigId}";
                            responseViewModels.Add(response);
                        }
                    }

                    //沒有任何回傳訊息(錯誤訊息)就更新資料庫
                    if (!responseViewModels.Any())
                    {
                        ResponseViewModel response = new();                        
                        accountant.AccountantSignGroupJournals.Add(accountantSignGroup);
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
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="accountantSignJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">建立或更新此檔的user的Id</param>
        private static void BaseInputAccountantSignJournal(AccountantSignJournal accountantSignJournal, bool isCreate , int userId)
        {
            if (isCreate)
            {
                accountantSignJournal.CreateUserId = userId;
                accountantSignJournal.CreateDate = DateTime.Now;
                accountantSignJournal.DeleteStatus = DeleteStatus.No;                                
            }
            else
            {
                accountantSignJournal.UpdateUserId = userId;
                accountantSignJournal.UpdateDate = DateTime.Now;                
            }
        }

        /// <summary>
        /// 會計師簽印建立日期歷程基本資料輸入
        /// </summary>
        /// <param name="accountantSignGroupJournal">會計師簽印建立日期歷程</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputSignGroupJournal(AccountantSignGroupJournal accountantSignGroupJournal, bool isCreate, int userId)
        {
            if (isCreate)
            {
                accountantSignGroupJournal.CreateUserId = userId;
                accountantSignGroupJournal.CreateDate = DateTime.Now;
                accountantSignGroupJournal.DeleteStatus = DeleteStatus.No;           
                accountantSignGroupJournal.ReviewStatus = ReviewStatus.Draft;
            }
            else
            {
                //如果變更的簽印組狀態是通過將結束日期更新為現在
                if (accountantSignGroupJournal.ReviewStatus == ReviewStatus.Approval)
                {
                    accountantSignGroupJournal.EndDate = DateTime.Now;
                }
                accountantSignGroupJournal.UpdateUserId = userId;
                accountantSignGroupJournal.UpdateDate = DateTime.Now;
                accountantSignGroupJournal.ReviewStatus = ReviewStatus.Disabled;
            }
        }

        /// <summary>
        /// 確認此類別會計師簽印是否重覆建立 true 重複 false 不重複
        /// </summary>
        /// <param name="accountantSignCheck">查詢參數</param>
        /// <param name="DeleteAccountantSignIds">異動中刪除的簽印Id</param>        
        /// <returns></returns>
        private bool CheckAccountSignRepeat(AccountantSignCheck accountantSignCheck, List<int> DeleteAccountantSignIds)
        {
            AccountantSignJournal? signQuery = dbContext.AccountantSignJournals
                                            .FirstOrDefault
                                            (
                                                accountantSignJournal => accountantSignJournal.AccountantSignGroupJournal.Id == accountantSignCheck.AccountantSignCreateDateJournalId
                                                && accountantSignJournal.ConfigType == accountantSignCheck.SealMappingConfigId
                                                && accountantSignJournal.DeleteStatus == DeleteStatus.No
                                                && !DeleteAccountantSignIds.Contains(accountantSignJournal.Id)                                                
                                            );
            return signQuery != null;
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
            AccountantSignGroupJournal? accountantSignGroupJournalQuery = dbContext.AccountantSignGroupJournals.Find(accountantSignGroupId);

            if(accountantSignGroupJournalQuery != null)
            {
                accountantSignGroupJournalQuery.ReviewStatus = reviewStatus;
                accountantSignGroupJournalQuery.UpdateUserId = userid;
                accountantSignGroupJournalQuery.UpdateDate = DateTime.Now;
                if(reviewStatus == ReviewStatus.Invalid)
                {
                    accountantSignGroupJournalQuery.DeleteStatus = DeleteStatus.Yes;
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
    }
}

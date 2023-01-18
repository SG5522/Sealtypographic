using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師簽印管理
    /// </summary>
    public class AcoountantSignService : IAccountantSignService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageSharpService;
        private readonly IMapper mapper;
        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageSharpService"></param>
        public AcoountantSignService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageSharpService)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
            this.imageSharpService = imageSharpService;

        }

        /// <summary>
        /// 取得會計師簽印建立日期列表
        /// </summary>
        /// <param name="accountantId">會計師Id</param>
        /// <returns></returns>
        public AccountantSignCreateDateViews GetCreateDates(int accountantId)
        {
            AccountantSignCreateDateViews accountantSignStartDates = new();
            accountantSignStartDates.GroupCreateDates = dbContext.SealReviewJournals
                                                       .Where
                                                       (
                                                            sealReviewJournal => sealReviewJournal.AccountantSignJournal.AccountantId == accountantId
                                                            && sealReviewJournal.ReviewStatus <= ReviewStatus.Draft
                                                            && sealReviewJournal.DeleteStatus == DeleteStatus.No
                                                       )
                                                       .Select(sealReviewJournal => new AccountantSignCreateDateView()
                                                       {
                                                           AccountantId = accountantId,
                                                           GroupCreateDate = sealReviewJournal.CreateDate,
                                                           ReviewStatus = sealReviewJournal.ReviewStatus
                                                       })
                                                       .GroupBy(accountantSignGroupCreateDateView => accountantSignGroupCreateDateView.GroupCreateDate)
                                                       .OrderByDescending(g => g.Key)
                                                       .Select(accountantSignGroupCreateDate => accountantSignGroupCreateDate.First())
                                                       .ToList();

            if (accountantSignStartDates.GroupCreateDates.Any())
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
        /// 依建立日期取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignCreateDate"></param>
        /// <returns></returns>
        public AccountantSignViewModels GetSignViewModels(AccountantSignCreateDate accountantSignCreateDate)
        {
            AccountantSignViewModels signViewModels = new();
            List<AccountantSignViewModel> accountantSignViewModels = new();

            signViewModels.AccountantId = accountantSignCreateDate.AccountantId;
            signViewModels.GroupCreateDate = accountantSignCreateDate.GroupCreateDate;            
            List<SealReviewJournal> sealReviewJournalQuery = dbContext.SealReviewJournals.Where
                                                                    (
                                                                        accountantSignJournal =>
                                                                        accountantSignJournal.AccountantSignJournal.AccountantId == accountantSignCreateDate.AccountantId                                                                        
                                                                        && accountantSignJournal.CreateDate == accountantSignCreateDate.GroupCreateDate
                                                                        && accountantSignJournal.DeleteStatus == DeleteStatus.No
                                                                        && accountantSignJournal.ReviewStatus <= ReviewStatus.Draft
                                                                    )
                                                                    .Include(sealReviewJournal => sealReviewJournal.AccountantSignJournal)
                                                                    .OrderBy(accountantSignJournal => accountantSignJournal.AccountantSignJournal.ConfigType)
                                                                    .ToList();

            if (sealReviewJournalQuery.Any())
            {
                foreach (SealReviewJournal sealReviewJournal in sealReviewJournalQuery)
                {
                    AccountantSignViewModel accountantSignViewModel = mapper.Map<AccountantSignViewModel>(sealReviewJournal);
                    accountantSignViewModel.ImageBase64 = imageSharpService.GetPathToBase64(sealReviewJournal.AccountantSignJournal.ImagePath, SealType.Accountant); //資料庫取得圖檔路徑轉BASE64                   
                    
                    accountantSignViewModel.SealMappingConfigId = sealReviewJournal.AccountantSignJournal.ConfigType;
                    accountantSignViewModels.Add(accountantSignViewModel);
                }
                signViewModels.ReviewStatus = sealReviewJournalQuery.First().ReviewStatus;
                signViewModels.SignViewModels = accountantSignViewModels;
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
            SealReviewJournal? sealReviewJournalQuery = dbContext.SealReviewJournals
                                .FirstOrDefault
                                (
                                    x => x.AccountantSignJournal.AccountantId == accountantSignForms.AccountantId                                    
                                    && x.ReviewStatus >= ReviewStatus.Draft
                                    && x.ReviewStatus <= ReviewStatus.Pending                                    
                                );
            if (sealReviewJournalQuery == null)
            {
                int count = 1;
                ImageBase64Info imageBase64Info = new()
                {
                    Code = GetCode(accountantSignForms.AccountantId),                    
                    SealType = SealType.Accountant
                };

                DateTime createNowTime = DateTime.Now;//將建立日期為依據將此次建立的會計師簽印組成為一組Group
                foreach (AccountantSignForm accountantSign in accountantSignForms.SignForms)
                {
                    AccountantSignJournal accountantSignJournal = new();
                    SealReviewJournal sealReviewJournal = new();
                                        
                    accountantSignJournal.AccountantId = accountantSignForms.AccountantId;
                    accountantSignJournal.ConfigType = (AccountantSignType)accountantSign.SealMappingConfigId;
                    imageBase64Info.ImageBase64 = accountantSign.ImageBase64;
                    accountantSignJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);                    
                    sealReviewJournal.CreateDate = createNowTime;

                    BaseInputAccountantSignJournal(accountantSignJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);

                    sealReviewJournal.AccountantSignJournal = accountantSignJournal;
                    dbContext.SealReviewJournals.Add(sealReviewJournal);
                }

                dbContext.SaveChanges();
                response.Success();
            }                   
            else
            {
                response.AccountantSignHaveDraftOrPendingReviewStatus();
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
            int count = 1;
            ImageBase64Info imageBase64Info = new()
            {
                Code = GetCode(accountantSignUpdate.AccountantId),
                SealType = SealType.Accountant,                
            };

            //刪除印鑑
            foreach (int sealReviewId in accountantSignUpdate.DeleteAccountantSignIds)
            {
                SealReviewJournal? deleteAccountantSignQuery = dbContext.SealReviewJournals
                                                                .Include(sealReview => sealReview.AccountantSignJournal)
                                                                .FirstOrDefault
                                                                (
                                                                    sealReview => sealReview.Id == sealReviewId
                                                                    && sealReview.DeleteStatus == DeleteStatus.No
                                                                    && sealReview.ReviewStatus <= ReviewStatus.Pending
                                                                );
                if (deleteAccountantSignQuery != null)
                {
                    deleteAccountantSignQuery.DeleteStatus = DeleteStatus.Yes;
                    deleteAccountantSignQuery.AccountantSignJournal.DeleteStatus = DeleteStatus.Yes;
                    BaseInputAccountantSignJournal(deleteAccountantSignQuery.AccountantSignJournal, false, userId);
                    BaseInputSealReviewJournal(deleteAccountantSignQuery, false, userId);
                }
                else
                {
                    ResponseViewModel response = new();
                    response.DeleteAccountantSignNoData();
                    response.ErrorItem = "Delete AccountantSignid: " + sealReviewId;
                    responseViewModels.Add(response);
                }
            }
            //修改印鑑
            foreach (AccountantSignFormUpdate accountantSignFormUpdate in accountantSignUpdate.UpdateAccountantSigns)
            {
                SealReviewJournal? updateSealQuery = dbContext.SealReviewJournals
                                                            .Include(sealReview => sealReview.AccountantSignJournal)
                                                            .FirstOrDefault
                                                            (
                                                                sealReview => sealReview.Id == accountantSignFormUpdate.Id
                                                                && sealReview.DeleteStatus == DeleteStatus.No      
                                                                && sealReview.ReviewStatus <= ReviewStatus.Pending
                                                            );
                if (updateSealQuery != null)
                {
                    SealReviewJournal sealReviewJournal = new()
                    {
                        CreateDate = accountantSignUpdate.GroupCreateDate
                    };
                    AccountantSignJournal accountantSignJournal = new()
                    {
                        AccountantId = accountantSignUpdate.AccountantId,
                        ConfigType = updateSealQuery.AccountantSignJournal.ConfigType,
                    };

                    //ImageBase64轉圖檔並存到指定資料夾                    
                    imageBase64Info.ImageBase64 = accountantSignFormUpdate.ImageBase64;
                    accountantSignJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);
                    
                    BaseInputAccountantSignJournal(accountantSignJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);

                    sealReviewJournal.AccountantSignJournal = accountantSignJournal;
                    dbContext.SealReviewJournals.Add(sealReviewJournal);

                    //原印鑑刪除(Hide)
                    updateSealQuery.DeleteStatus = DeleteStatus.Yes;
                    updateSealQuery.AccountantSignJournal.DeleteStatus = DeleteStatus.Yes;
                    BaseInputAccountantSignJournal(updateSealQuery.AccountantSignJournal, false, userId);
                    BaseInputSealReviewJournal(updateSealQuery, false, userId);                    

                    count++;
                }
                else
                {
                    ResponseViewModel response = new();
                    response.UpdateAccountantSignNoData();
                    response.ErrorItem = "Update AccountantSignid: " + accountantSignFormUpdate.Id;
                    responseViewModels.Add(response);
                }
            }
            //新增印鑑
            foreach (AccountantSignForm createAccountantSign in accountantSignUpdate.CreateAccountantSigns)
            {
                AccountantSignCheck accountantSignCheck = new() 
                {
                    AccountantId = accountantSignUpdate.AccountantId,
                    SealMappingConfigId = createAccountantSign.SealMappingConfigId,
                    GroupCreateDate = accountantSignUpdate.GroupCreateDate         
                };                
                
                if (!CheckAccountSignRepeat(accountantSignCheck, accountantSignUpdate.DeleteAccountantSignIds))
                {
                    AccountantSignJournal accountantSignJournal = new()
                    {
                        AccountantId = accountantSignUpdate.AccountantId,
                        ConfigType = createAccountantSign.SealMappingConfigId
                    };
                    SealReviewJournal sealReviewJournal = new()
                    {
                        CreateDate = accountantSignUpdate.GroupCreateDate
                    };
                    
                    imageBase64Info.ImageBase64 = createAccountantSign.ImageBase64;
                    accountantSignJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);
                    
                    BaseInputAccountantSignJournal(accountantSignJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);

                    sealReviewJournal.AccountantSignJournal = accountantSignJournal;
                    dbContext.SealReviewJournals.Add(sealReviewJournal);

                    count++;
                    //accountantSignJournals.Add(accountantSignJournal);
                }
                else
                {
                    ResponseViewModel response = new();
                    response.CreateAccountantSignRepeat();
                    response.ErrorItem = "New AccountantId:" + accountantSignUpdate.AccountantId
                                       + " SealMappingConfigId:" + createAccountantSign.SealMappingConfigId;
                    responseViewModels.Add(response);
                }
            }

            //沒有任何回傳訊息(錯誤訊息)就更新資料庫
            if (!responseViewModels.Any())
            {
                ResponseViewModel response = new();

                //將此創建日期的簽印審查狀態全變更為草稿(更新時需要重審)
                IQueryable<SealReviewJournal> sealReviewJournalQuery = dbContext.SealReviewJournals.Where
                                                (
                                                    sealReview => sealReview.AccountantSignJournal.AccountantId == accountantSignUpdate.AccountantId                                                    
                                                    && sealReview.CreateDate == accountantSignUpdate.GroupCreateDate
                                                    && sealReview.DeleteStatus == DeleteStatus.No
                                                );
                foreach(SealReviewJournal sealReviewJournal in sealReviewJournalQuery)
                {
                    sealReviewJournal.ReviewStatus = ReviewStatus.Draft;
                }


                dbContext.SaveChanges();
                response.Success();
                responseViewModels.Add(response);
            }

            return responseViewModels;
        }


        /// <summary>
        /// 將草稿的簽印組狀態變更為待審
        /// </summary>
        /// <param name="accountantSignCreateDate">會計師簽印搜尋(依會計師ID與創建群組日期)</param>        
        /// <returns></returns>
        public ResponseViewModel PendingSigns(AccountantSignCreateDate accountantSignCreateDate)
        {            
            ResponseViewModel response = ChangeDraftReviewStatus(accountantSignCreateDate, ReviewStatus.Pending);
            return response;
        }

        /// <summary>
        /// 將草稿的簽印組狀態變更為作廢
        /// </summary>
        /// <param name="accountantSignCreateDate">會計師簽印搜尋(依會計師ID與創建群組日期)</param>        
        public ResponseViewModel InvalidSigns(AccountantSignCreateDate accountantSignCreateDate)
        {
            ResponseViewModel response = ChangeDraftReviewStatus(accountantSignCreateDate, ReviewStatus.Invalid);
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
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="sealReviewJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputSealReviewJournal(SealReviewJournal sealReviewJournal, bool isCreate, int userId)
        {
            if (isCreate)
            {
                sealReviewJournal.CreateUserId = userId;                
                sealReviewJournal.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                sealReviewJournal.UpdateUserId = userId;
                sealReviewJournal.UpdateDate = DateTime.Now;
            }
            sealReviewJournal.StartDate = AvailableDateUtil.NotActivated();
            sealReviewJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            sealReviewJournal.ReviewStatus = ReviewStatus.Draft;
        }

        /// <summary>
        /// 確認此類別會計師簽印是否重覆建立 true 重複 false 不重複
        /// </summary>
        /// <param name="accountantSignCheck">查詢參數</param>
        /// <param name="DeleteAccountantSignIds">異動中刪除的簽印</param>
        /// <returns></returns>
        private bool CheckAccountSignRepeat(AccountantSignCheck accountantSignCheck ,List<int> DeleteAccountantSignIds)
        {
            SealReviewJournal? sealQuery = dbContext.SealReviewJournals
                                            .FirstOrDefault
                                            (
                                                sealReviewJournal => sealReviewJournal.AccountantSignJournal.AccountantId == accountantSignCheck.AccountantId
                                                && sealReviewJournal.AccountantSignJournal.ConfigType == accountantSignCheck.SealMappingConfigId
                                                && sealReviewJournal.CreateDate == accountantSignCheck.GroupCreateDate
                                                && sealReviewJournal.DeleteStatus == DeleteStatus.No
                                                && sealReviewJournal.ReviewStatus <= ReviewStatus.Pending
                                                && !DeleteAccountantSignIds.Contains(sealReviewJournal.Id)
                                            );
            return sealQuery != null;
        }

        /// <summary>
        /// 會計師印鑑待審狀態變更。
        /// </summary>
        /// <param name="accountantSignCreateDate"></param>
        /// <param name="reviewStatus">審查狀態</param>        
        private ResponseViewModel ChangeDraftReviewStatus(AccountantSignCreateDate accountantSignCreateDate, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userid = 0; //從帳號驗證取得Id
            IQueryable<SealReviewJournal>? sealReviewJournalQuery = dbContext.SealReviewJournals
                                                                        .Where
                                                                        (
                                                                            sealReviewJournal => 
                                                                            sealReviewJournal.AccountantSignJournal.AccountantId == accountantSignCreateDate.AccountantId
                                                                            && sealReviewJournal.CreateDate == accountantSignCreateDate.GroupCreateDate
                                                                            && sealReviewJournal.DeleteStatus == DeleteStatus.No
                                                                            && sealReviewJournal.ReviewStatus == ReviewStatus.Draft
                                                                        );
            if(sealReviewJournalQuery.Any())
            {
                foreach (SealReviewJournal sealReview in sealReviewJournalQuery)
                {
                    sealReview.ReviewStatus = reviewStatus;
                    sealReview.UpdateUserId = userid;
                    sealReview.UpdateDate = DateTime.Now;
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
        private string GetCode(int accountantId)
        {
            string code;
            Accountant? accountant = dbContext.Accountants.Find(accountantId);
            if (accountant != null)
            {
                code = accountant.Code;
            }
            else
            {
                code = string.Empty;
            }
            return code;
        }
    }
}

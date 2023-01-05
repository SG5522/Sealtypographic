using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Utils;
using System.Collections.Generic;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用的顧客印鑑組
    /// </summary>
    public class AcoountantSignService : IAccountantSignService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;
        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        public AcoountantSignService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="accountantId">搜尋條件</param>
        /// <returns></returns>
        public AccountantSignGroupCreateDateViews GetAccountantWithGruopCreateDate(int accountantId)
        {
            AccountantSignGroupCreateDateViews accountantSignStartDates = new();
            List<AccountantSignGroupCreateDateView> signGroupCreateDate = dbContext.AccountantSignJournals
                                           .Where
                                           (
                                                accountantSignJournal => accountantSignJournal.AccountantId == accountantId
                                                && accountantSignJournal.ReviewStatus <= ReviewStatus.Draft //暂存以下狀態過濾用
                                           )
                                           .Select(accountantSignJournal => new AccountantSignGroupCreateDateView()
                                           {
                                               AccountantId = accountantSignJournal.AccountantId,
                                               GroupCreateDate = accountantSignJournal.GroupCreateDate,
                                               ReviewStatus = accountantSignJournal.ReviewStatus
                                           })
                                           .GroupBy(accountantSignJournal => accountantSignJournal.GroupCreateDate)
                                           .OrderByDescending(g => g.Key)
                                           .Select(accountantSignJournal => accountantSignJournal.First())
                                           .ToList();

            if (signGroupCreateDate.Any())
            {                
                accountantSignStartDates.GroupCreateDates = signGroupCreateDate;                
                accountantSignStartDates.Success();
            }
            else
            {
                accountantSignStartDates.AccountantSignNoData();
            }
            return accountantSignStartDates;
        }

        /// <summary>
        /// 取得會計師簽印組()
        /// </summary>
        /// <param name="accountantSignGroupCreateDateSearch"></param>
        /// <returns></returns>
        public AccountantSignViewModels GetAccountantSings(AccountantSignGroupCreateDateSearch accountantSignGroupCreateDateSearch)
        {
            AccountantSignViewModels signViewModels = new();
            List<AccountantSignViewModel> accountantSignViewModels = new();
            signViewModels.AccountantId = accountantSignGroupCreateDateSearch.AccountantId;
            signViewModels.GroupCreateDate = accountantSignGroupCreateDateSearch.GroupCreateDate;            
            List<AccountantSignJournal> accountantSignJournalQuery = dbContext.AccountantSignJournals.Where
                                                                    (
                                                                        accountantSignJournal =>
                                                                        accountantSignJournal.AccountantId == accountantSignGroupCreateDateSearch.AccountantId
                                                                        && accountantSignJournal.DeleteStatus == DeleteStatus.NO
                                                                        && accountantSignJournal.GroupCreateDate == accountantSignGroupCreateDateSearch.GroupCreateDate
                                                                        && accountantSignJournal.ReviewStatus <= ReviewStatus.Draft
                                                                    )                                                                    
                                                                    .OrderBy(accountantSignJournal => accountantSignJournal.ConfigType)
                                                                    .ToList();

            if (accountantSignJournalQuery.Any())
            {
                foreach (AccountantSignJournal accountantSignJournal in accountantSignJournalQuery)
                {
                    AccountantSignViewModel customerSealViewModel = mapper.Map<AccountantSignViewModel>(accountantSignJournal);
                    customerSealViewModel.ImageBase64 = accountantSignJournal.ImagePath; //之後會在做BASE64轉換
                    accountantSignViewModels.Add(customerSealViewModel);
                }
                signViewModels.ReviewStatus = accountantSignJournalQuery.First().ReviewStatus;
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
        /// <param name="accountantSignPostDatas">簽名印鑑組</param>
        /// <returns></returns>
        public ResponseViewModel CreateAccountantSigns(List<AccountantSignForm> accountantSignPostDatas)
        {
            ResponseViewModel response = new();
            List<AccountantSignJournal> accountantSignJournals = new();
            int userId = 0; //從帳號驗證取得Id
            //確認是否有一組草稿或待審的會計師簽印
            AccountantSignJournal? accountantSignJournalQuery = dbContext.AccountantSignJournals
                                .FirstOrDefault
                                (
                                    x => x.AccountantId == accountantSignPostDatas.First().AccountantId
                                    && x.ReviewStatus >= ReviewStatus.Draft
                                    && x.ReviewStatus <= ReviewStatus.Pending                                    
                                );
            if (accountantSignJournalQuery == null)
            {
                DateTime createNowTime = DateTime.Now;//將建立日期為依據將此次建立的會計師簽印組成為一組Group
                foreach (AccountantSignForm accountantSignPostData in accountantSignPostDatas)
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = accountantSignPostData.ImageBase64;

                    AccountantSignJournal accountantSignJournal = mapper.Map<AccountantSignJournal>(accountantSignPostData);
                    accountantSignJournal.ImagePath = imagePath;
                    accountantSignJournal.GroupCreateDate = createNowTime;
                    BaseInputAccountantSignJournal(accountantSignJournal, true, userId);                                                            
                    accountantSignJournals.Add(accountantSignJournal);
                }
                dbContext.AccountantSignJournals.AddRange(accountantSignJournals);
                dbContext.BulkSaveChanges();
                response.Success();
            }                   
            else
            {
                response.AccountantSignHaveDraftOrPendingReviewStatus();
            }
            return response;
        }
        /// <summary>
        /// 異動會計師簽印的處理(審查狀態退回或是草稿才進行修改)
        /// </summary>
        /// <param name="accountantSignUpdate">刪除修改新增的list</param>
        /// <returns></returns>
        public List<ResponseViewModel> UpdateAccountantSign(AccountantSignUpdate accountantSignUpdate)
        {
            List<ResponseViewModel> responseViewModels = new();
            List<AccountantSignJournal> accountantSignJournals = new();
            int userId = 0;//之後會從帳號驗證中取得userid
            //刪除印鑑
            foreach (int accountantSignId in accountantSignUpdate.DeleteAccountantSignIds)
            {
                AccountantSignJournal? deleteAccountantSignQuery = dbContext.AccountantSignJournals.FirstOrDefault
                                                                (
                                                                    accountantSign => accountantSign.Id == accountantSignId
                                                                    && accountantSign.DeleteStatus == DeleteStatus.NO
                                                                    && accountantSign.ReviewStatus <= ReviewStatus.Draft
                                                                );
                if (deleteAccountantSignQuery != null)
                {
                    deleteAccountantSignQuery.DeleteStatus = DeleteStatus.Yes;
                    deleteAccountantSignQuery.UpdateDate = DateTime.Now;
                    deleteAccountantSignQuery.UpdateUserId = userId; 
                }
                else
                {
                    ResponseViewModel response = new();
                    response.DeleteAccountantSignNoData();
                    response.ErrorItem = "Delete AccountantSignid: " + accountantSignId;
                    responseViewModels.Add(response);
                }
            }
            //修改印鑑
            foreach (AccountantSignFormUpdate accountantSignFormUpdate in accountantSignUpdate.UpdateAccountantSigns)
            {
                AccountantSignJournal? updateAccountantSignQuery = dbContext.AccountantSignJournals.FirstOrDefault
                                                            (
                                                                accountantSign => accountantSign.Id == accountantSignFormUpdate.Id
                                                                && accountantSign.DeleteStatus == DeleteStatus.NO      
                                                                && accountantSign.ReviewStatus <= ReviewStatus.Draft
                                                            );
                if (updateAccountantSignQuery != null)
                {
                    //mapper.Map(accountantSignFormUpdate, updateAccountantSignQuery);
                    string imagePath = accountantSignFormUpdate.ImageBase64;//這段之後會做成IMAGE64的處理並另存在指定的位置

                    updateAccountantSignQuery.ImagePath = imagePath;
                    BaseInputAccountantSignJournal(updateAccountantSignQuery, false, userId);
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
            foreach (AccountantSignForm accountantSignForm in accountantSignUpdate.CreateAccountantSigns)
            {
                //這段之後會做成IMAGE64的處理並另存在指定的位置
                string imagePath = accountantSignForm.ImageBase64;
                AccountantSignCheck accountantSignCheck = new() 
                {
                    AccountantId = accountantSignForm.AccountantId,
                    SealMappingConfigId = accountantSignForm.SealMappingConfigId,
                    GroupCreateDate = accountantSignUpdate.GroupCreateDate         
                };                
                
                if (!CheckAccountSignRepeat(accountantSignCheck, accountantSignUpdate.DeleteAccountantSignIds))
                {
                    AccountantSignJournal accountantSignJournal = mapper.Map<AccountantSignJournal>(accountantSignForm);
                    accountantSignJournal.ImagePath = imagePath;
                    accountantSignJournal.GroupCreateDate = accountantSignUpdate.GroupCreateDate;
                    BaseInputAccountantSignJournal(accountantSignJournal, true, userId);
                    accountantSignJournals.Add(accountantSignJournal);
                }
                else
                {
                    ResponseViewModel response = new();
                    response.CreateAccountantSignRepeat();
                    response.ErrorItem = "Create AccountantId:" + accountantSignForm.AccountantId
                                       + " SealMappingConfigId:" + accountantSignForm.SealMappingConfigId;
                    responseViewModels.Add(response);
                }
            }

            //沒有任何回傳訊息(錯誤訊息)就更新資料庫
            if (!responseViewModels.Any())
            {
                ResponseViewModel response = new();

                //將此創建日期的簽印審查狀態全變更為草稿(更新時需要重審)
                IQueryable<AccountantSignJournal> accountantSignJournalQuery = dbContext.AccountantSignJournals.Where
                                                (
                                                    accountantSign => accountantSign.AccountantId == accountantSignUpdate.AccountantId                                                    
                                                    && accountantSign.GroupCreateDate == accountantSignUpdate.GroupCreateDate
                                                    && accountantSign.DeleteStatus == DeleteStatus.NO
                                                );
                foreach(AccountantSignJournal accountantSignJournal in accountantSignJournalQuery)
                {
                    accountantSignJournal.ReviewStatus = ReviewStatus.Draft;
                }

                dbContext.AccountantSignJournals.AddRange(accountantSignJournals);
                dbContext.BulkSaveChanges();
                response.Success();
                responseViewModels.Add(response);
            }

            return responseViewModels;
        }


        /// <summary>
        /// 變更此群組創建日期的會計師簽印為待審
        /// </summary>
        /// <param name="accountantSignGroupCreateDateSearch">會計師簽印群組創建日期</param>        
        /// <returns></returns>
        public ResponseViewModel UpdateReviewStatusPendingAccountantSigns(AccountantSignGroupCreateDateSearch accountantSignGroupCreateDateSearch)
        {            
            ResponseViewModel response = ChangeTempReviewStatusAccountantSigns(accountantSignGroupCreateDateSearch, ReviewStatus.Pending);
            return response;
        }

        /// <summary>
        /// 變更此群組創建日期的會計師簽印為作廢。
        /// </summary>
        /// <param name="accountantSignGroupCreateDateSearch">會計師簽印群組創建日期</param>        
        public ResponseViewModel UpdateReviewStatusInvalidAccountantSigns(AccountantSignGroupCreateDateSearch accountantSignGroupCreateDateSearch)
        {
            ResponseViewModel response = ChangeTempReviewStatusAccountantSigns(accountantSignGroupCreateDateSearch, ReviewStatus.Invalid);
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
                accountantSignJournal.DeleteStatus = DeleteStatus.NO;                                
            }
            else
            {
                accountantSignJournal.UpdateUserId = userId;
                accountantSignJournal.UpdateDate = DateTime.Now;                
            }
            accountantSignJournal.StartDate = AvailableDateUtil.NotActivated();
            accountantSignJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            accountantSignJournal.ReviewStatus = ReviewStatus.Draft; //建立或是更新簽印都會變成草稿狀態
        }

        /// <summary>
        /// 確認此類別會計師簽印是否重覆建立 true 重複 false 不重複
        /// </summary>
        /// <param name="accountantSignCheck">查詢參數</param>
        /// <param name="DeleteAccountantSignIds">異動中刪除的簽印</param>
        /// <returns></returns>
        private bool CheckAccountSignRepeat(AccountantSignCheck accountantSignCheck ,List<int> DeleteAccountantSignIds)
        {
            AccountantSignJournal? AccountantSignQuery = dbContext.AccountantSignJournals
                                                        .FirstOrDefault
                                                        (
                                                            accountantSign => accountantSign.AccountantId == accountantSignCheck.AccountantId
                                                            && accountantSign.ConfigType == (AccountantSignConfigType)accountantSignCheck.SealMappingConfigId
                                                            && accountantSign.GroupCreateDate == accountantSignCheck.GroupCreateDate
                                                            && accountantSign.DeleteStatus == DeleteStatus.NO
                                                            && accountantSign.ReviewStatus <= ReviewStatus.Pending       
                                                            && !DeleteAccountantSignIds.Contains(accountantSign.Id)
                                                        );
            return AccountantSignQuery != null;
        }

        /// <summary>
        /// 會計師印鑑待審狀態變更。
        /// </summary>
        /// <param name="accountantSignGroupCreateDateSearch"></param>
        /// <param name="reviewStatus">審查狀態</param>        
        private ResponseViewModel ChangeTempReviewStatusAccountantSigns(AccountantSignGroupCreateDateSearch accountantSignGroupCreateDateSearch, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userid = 0; //從帳號驗證取得Id
            List<AccountantSignJournal>? accountantSignJournalQuery = dbContext.AccountantSignJournals.Where
                                                                (
                                                                    accountantSignJournal => accountantSignJournal.AccountantId == accountantSignGroupCreateDateSearch.AccountantId
                                                                    && accountantSignJournal.GroupCreateDate == accountantSignGroupCreateDateSearch.GroupCreateDate
                                                                    && accountantSignJournal.DeleteStatus == DeleteStatus.NO
                                                                    && accountantSignJournal.ReviewStatus == ReviewStatus.Draft
                                                                ).ToList();
            if(accountantSignJournalQuery.Any())
            {
                foreach (AccountantSignJournal accountantSign in accountantSignJournalQuery)
                {
                    accountantSign.ReviewStatus = reviewStatus;
                    accountantSign.UpdateUserId = userid;
                    accountantSign.UpdateDate = DateTime.Now;
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

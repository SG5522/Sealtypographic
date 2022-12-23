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
        public AccountantSignGroupCreateDates GetAccountantWithGruopCreateDate(int accountantId)
        {
            AccountantSignGroupCreateDates accountantSignStartDates = new();
            List<AccountantSignGroupCreateDate> signGroupCreateDate = new();
            List<DateTime> accountantSignCreateQuery = dbContext.AccountantSignJournals
                                           .Where
                                           (
                                                accountantSignJournal => accountantSignJournal.AccountantId == accountantId
                                                && accountantSignJournal.ReviewStatus <= ReviewStatus.Draft //暂存以下狀態過濾用
                                           )
                                           .Select(accountantSignJournal => accountantSignJournal.GroupCreateDate)
                                           .Distinct()
                                           .ToList();

            if (accountantSignCreateQuery.Any())
            {
                foreach (DateTime createDate in accountantSignCreateQuery)
                {
                    signGroupCreateDate.Add(new()
                    {
                        AccountantId = accountantId,
                        GroupCreateDate = createDate
                    });
                }
                accountantSignStartDates.GroupCreateDates = signGroupCreateDate;
                accountantSignStartDates.Success();
            }
            else
            {
                accountantSignStartDates.DbNoData();
            }

            return accountantSignStartDates;
        }

        /// <summary>
        /// 取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignStartDate"></param>
        /// <returns></returns>
        public AccountantSignViewModels GetAccountantSings(AccountantSignGroupCreateDate accountantSignStartDate)
        {
            AccountantSignViewModels signViewModels = new();
            List<AccountantSignViewModel> accountantSignViewModels = new();
            List<AccountantSignJournal> accountantSignJournalQuery = dbContext.AccountantSignJournals.Where
                                                                    (
                                                                        accountantSignJournal =>
                                                                        accountantSignJournal.AccountantId == accountantSignStartDate.AccountantId
                                                                        && accountantSignJournal.DeleteStatus == DeleteStatus.NO
                                                                        && accountantSignJournal.GroupCreateDate == accountantSignStartDate.GroupCreateDate
                                                                        && accountantSignJournal.ReviewStatus <= ReviewStatus.Draft
                                                                    )
                                                                    .Include(accountantSignJournal => accountantSignJournal.SealMappingConfig)
                                                                    .OrderBy(accountantSignJournal => accountantSignJournal.SealMappingConfigId)
                                                                    .ToList();

            if (accountantSignJournalQuery.Any())
            {
                foreach (AccountantSignJournal accountantSignJournal in accountantSignJournalQuery)
                {
                    AccountantSignViewModel customerSealViewModel = mapper.Map<AccountantSignViewModel>(accountantSignJournal);
                    customerSealViewModel.ImageBase64 = accountantSignJournal.ImagePath; //之後會在做BASE64轉換
                    accountantSignViewModels.Add(customerSealViewModel);
                }
                signViewModels.SignViewModels = accountantSignViewModels;
                signViewModels.Success();
            }
            else
            {
                signViewModels.DbNoData();
            }

            return signViewModels;
        }

        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="accountantSignPostDatas">簽名印鑑組</param>
        /// <returns></returns>
        public ResponseViewModel CreateAccountantSigns(List<AccountantSignForm> accountantSignPostDatas)
        {
            ResponseViewModel response = new();
            List<AccountantSignJournal> accountantSignJournals = new();

            AccountantSignJournal? accountantSignJournalQuery = dbContext.AccountantSignJournals
                                .FirstOrDefault
                                (
                                    x => x.AccountantId == accountantSignPostDatas.First().AccountantId
                                    && x.ReviewStatus == ReviewStatus.Draft
                                );            
            if(accountantSignJournalQuery == null)
            {
                DateTime createNowTime = DateTime.Now;//將建立日期為依據將此次建立的會計師簽印組成為一組Group
                foreach (AccountantSignForm accountantSignPostData in accountantSignPostDatas)
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = accountantSignPostData.ImageBase64;

                    AccountantSignJournal accountantSignJournal = mapper.Map<AccountantSignJournal>(accountantSignPostData);

                    accountantSignJournal.ImagePath = imagePath;
                    BaseInputAccountantSignJournal(accountantSignJournal, true, 0);                                              
                    accountantSignJournal.GroupCreateDate = createNowTime;                                                                                 
                    accountantSignJournals.Add(accountantSignJournal);
                }
                dbContext.AccountantSignJournals.AddRange(accountantSignJournals);
                dbContext.BulkSaveChanges();
                response.Success();
            }                   
            else
            {
                response.AccountantSignHaveTemp();
            }
            return response;
        }
        /// <summary>
        /// 異動會計師簽印的處理(退回或是草稿修改)
        /// </summary>
        /// <param name="accountantSignUpdate">刪除修改新增的list</param>
        /// <returns></returns>
        public List<ResponseViewModel> UpdateAccountantSign(AccountantSignUpdate accountantSignUpdate)
        {
            List<ResponseViewModel> responseViewModels = new();
            List<AccountantSignJournal> accountantSignJournals = new();
            //刪除印鑑
            foreach (int accountantSignId in accountantSignUpdate.DeleteAccountantSignIds)
            {
                AccountantSignJournal? deleteAccountantSignQuery = dbContext.AccountantSignJournals.FirstOrDefault
                                                                (
                                                                    accountantSign => accountantSign.Id == accountantSignId
                                                                    && accountantSign.DeleteStatus == DeleteStatus.NO
                                                                );
                if (deleteAccountantSignQuery != null)
                {
                    deleteAccountantSignQuery.DeleteStatus = DeleteStatus.Yes;
                    deleteAccountantSignQuery.UpdateDate = DateTime.Now;
                    deleteAccountantSignQuery.UpdateUserId = 0; //未來從帳號驗證中取得userid
                }
                else
                {
                    ResponseViewModel response = new();
                    response.AccountantSignNoData();
                    response.ErrorItem = "Delete AccountantSignid:" + accountantSignId;
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
                                                            );
                if (updateAccountantSignQuery != null)
                {
                    mapper.Map(accountantSignFormUpdate, updateAccountantSignQuery);
                    string imagePath = accountantSignFormUpdate.ImageBase64;//這段之後會做成IMAGE64的處理並另存在指定的位置

                    updateAccountantSignQuery.ImagePath = imagePath;
                    BaseInputAccountantSignJournal(updateAccountantSignQuery, false, 0);
                }
                else
                {
                    ResponseViewModel response = new();
                    response.UpdateAccountantSignNoData();
                    response.ErrorItem = "Update AccountantSignId:" + accountantSignFormUpdate.Id;
                    responseViewModels.Add(response);
                }
            }
            //新增印鑑
            foreach (AccountantSignForm accountantSignForm in accountantSignUpdate.CreateAccountantSigns)
            {
                //這段之後會做成IMAGE64的處理並另存在指定的位置
                string imagePath = accountantSignForm.ImageBase64;

                AccountantSignJournal accountantSignJournal = mapper.Map<AccountantSignJournal>(accountantSignForm);
                accountantSignJournal.ImagePath = imagePath;
                accountantSignJournal.GroupCreateDate = accountantSignUpdate.GroupCreateDate;
                BaseInputAccountantSignJournal(accountantSignJournal, true, 0);
                accountantSignJournals.Add(accountantSignJournal);
                if(CheckAccountSignRepeat())
                {

                }
                else
                {
                    ResponseViewModel response = new();
                    response.AccountantSignRepeat();
                    response.ErrorItem = "Create AccountantId:" + accountantSignForm.AccountantId
                                       + " SealMappingConfigId:" + accountantSignForm.SealMappingConfigId;                                       
                    responseViewModels.Add(response);
                }
            }
            //更新資料庫
            if (!responseViewModels.Any())
            {
                ResponseViewModel response = new();
                dbContext.CustomerSealJournals.AddRange(accountantSignJournals);
                dbContext.BulkSaveChanges();
                response.Success();
                responseViewModels.Add(response);
            }

            return responseViewModels;
        }


        /// <summary>
        /// 修改印鑑組
        /// </summary>
        /// <param name="accountantSignIds">會計師簽印Id</param>        
        /// <returns></returns>
        public List<ResponseViewModel> UpdateReviewStatusPendingAccountantSigns(List<int> accountantSignIds)
        {            
            List<ResponseViewModel> responseViewModels = ChangeTempReviewStatusAccountantSigns(accountantSignIds, ReviewStatus.Pending);
            return responseViewModels;
        }
        
        /// <summary>
        /// 變更此客戶狀態為刪除。
        /// </summary>
        /// <param name="accountantSignIds">會計師簽印Id</param>        
        public List<ResponseViewModel> DeleteAccountantSign(List<int> accountantSignIds)
        {
            List<ResponseViewModel> responseViewModels = ChangeTempReviewStatusAccountantSigns(accountantSignIds, ReviewStatus.Invalid);
            return responseViewModels;
        }

        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="accountantSignJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">建立或更新此檔的user的id 這段先設定為0 以後從驗證中取得userid</param>
        private static void BaseInputAccountantSignJournal(AccountantSignJournal accountantSignJournal, bool isCreate , int userId)
        {
            if (isCreate)
            {
                accountantSignJournal.CreateDate = DateTime.Now;
                accountantSignJournal.DeleteStatus = DeleteStatus.NO;                
                accountantSignJournal.CreateUserId = userId;
            }
            else
            {
                accountantSignJournal.UpdateDate = DateTime.Now;
                accountantSignJournal.UpdateUserId = userId;
            }
            accountantSignJournal.StartDate = AvailableDateUtil.NotActivated();
            accountantSignJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            accountantSignJournal.ReviewStatus = ReviewStatus.Draft;
        }

        /// <summary>
        /// 確認客戶序號是否重複 true 不重複 false 重複
        /// </summary>
        /// <param name="accountantSignCheck">查詢參數</param>
        /// <returns></returns>
        private bool CheckAccountSignRepeat(AccountantSignCheck accountantSignCheck)
        {
            AccountantSignJournal? AccountantSignQuery = dbContext.AccountantSignJournals.FirstOrDefault
                                                        (
                                                            accountantSign => accountantSign.AccountantId == accountantSignCheck.AccountantId
                                                            && accountantSign.SealMappingConfigId == accountantSignCheck.SealMappingConfigId
                                                            && accountantSign.GroupCreateDate == accountantSignCheck.GroupCreateDate
                                                            && accountantSign.DeleteStatus == DeleteStatus.NO
                                                        );
            return AccountantSignQuery == null;
        }

        /// <summary>
        /// 會計師印鑑待審狀態變更。
        /// </summary>
        /// <param name="accountantSignIds">會計師簽印Id</param>
        /// <param name="reviewStatus">審查狀態</param>        
        private List<ResponseViewModel> ChangeTempReviewStatusAccountantSigns(List<int> accountantSignIds, ReviewStatus reviewStatus)
        {
            List<ResponseViewModel> responseViewModels = new();
            foreach (int accountantSignId in accountantSignIds)
            {
                AccountantSignJournal? accountantSignJournalQuery = dbContext.AccountantSignJournals.FirstOrDefault
                                                                    (
                                                                        accountantSignJournal => accountantSignJournal.Id == accountantSignId
                                                                        && accountantSignJournal.ReviewStatus == ReviewStatus.Draft
                                                                    );
                ResponseViewModel response = new();
                if (accountantSignJournalQuery != null)
                {
                    accountantSignJournalQuery.ReviewStatus = reviewStatus;
                }
                else
                {
                    response.DbNoData();
                    response.ErrorItem = "accountantSignId:" + accountantSignId;
                    responseViewModels.Add(response);
                }
            }
            if (responseViewModels.Count == 0)
            {
                ResponseViewModel response = new();
                dbContext.SaveChanges();
                response.Success();
                responseViewModels.Add(response);
            }
            return responseViewModels;
        }
    }
}

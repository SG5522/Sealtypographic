using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
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
        public AccountantSignCreateDates GetAccountantStartDate(int accountantId)
        {
            AccountantSignCreateDates accountantSignStartDates = new();
            List<AccountantSignCreateDate> signCreateDate = new();
            List<DateTime> accountantSignCreateQuery = dbContext.AccountantSignJournals
                                           .Where
                                           (
                                                accountantSignJournal => accountantSignJournal.AccountantId == accountantId
                                                && accountantSignJournal.ReviewStatus <= ReviewStatus.Temp //暂存以下狀態過濾用
                                           )
                                           .Select(accountantSignJournal => accountantSignJournal.CreateDate)
                                           .Distinct()
                                           .ToList();

            if (accountantSignCreateQuery.Any())
            {
                foreach (DateTime createDate in accountantSignCreateQuery)
                {
                    signCreateDate.Add(new()
                    {
                        AccountantId = accountantId,
                        CreateDate = createDate
                    });
                }
                accountantSignStartDates.CreateDates = signCreateDate;
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
        public AccountantSignViewModels GetAccountantSings(AccountantSignCreateDate accountantSignStartDate)
        {
            AccountantSignViewModels signViewModels = new();
            List<AccountantSignViewModel> accountantSignViewModels = new();
            List<AccountantSignJournal> accountantSignJournalQuery = dbContext.AccountantSignJournals.Where
                                                                    (
                                                                        accountantSignJournal =>
                                                                        accountantSignJournal.AccountantId == accountantSignStartDate.AccountantId
                                                                        && accountantSignJournal.DeleteStatus == DeleteStatus.NO
                                                                        && accountantSignJournal.CreateDate == accountantSignStartDate.CreateDate
                                                                        //&& accountantSignJournal.ReviewStatus > ReviewStatus.Approval
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
                                    && x.ReviewStatus == ReviewStatus.Temp
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
                    accountantSignJournal.DeleteStatus = DeleteStatus.NO;
                    accountantSignJournal.ReviewStatus = ReviewStatus.Temp;
                    accountantSignJournal.CreateDate = createNowTime;
                    //accountantSignJournal.StartDate = AvailableDateUtil.NotActivated();
                    //accountantSignJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                                                                                    
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
            List<ResponseViewModel> responseViewModels = ChangeTempReviewStatusAccountantSigns(accountantSignIds, ReviewStatus.SealsVoid);
            return responseViewModels;
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
                                                                        && accountantSignJournal.ReviewStatus == ReviewStatus.Temp
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

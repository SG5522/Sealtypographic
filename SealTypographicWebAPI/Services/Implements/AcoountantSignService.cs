using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Utils;

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
        public AccountantSignViewModels GetAccountantSings(int accountantId)
        {
            AccountantSignViewModels signViewModels = new();
            List<AccountantSignViewModel> accountantSignViewModels = new();            
            List<AccountantSignJournal> accountantSignJournalQuery = dbContext.AccountantSignJournals.Where
                                                                    (
                                                                        accountantSignJournal => 
                                                                        accountantSignJournal.AccountantId == accountantId   
                                                                        && accountantSignJournal.DeleteStatus == DeleteStatus.NO
                                                                        //&& accountantSignJournal.ReviewStatus == ReviewStatus.Approval
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
            foreach (AccountantSignForm accountantSignPostData in accountantSignPostDatas)
            {
                if(CheckRepeatSealMappingConfigId(mapper.Map<AccountantSignCheck>(accountantSignPostData)))
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = accountantSignPostData.ImageBase64;

                    AccountantSignJournal accountantSignJournal = mapper.Map<AccountantSignJournal>(accountantSignPostData);
                    accountantSignJournal.ImagePath = imagePath;
                    accountantSignJournal.CreateDate = DateTime.Now;
                    accountantSignJournal.ReviewStatus = Consts.ReviewStatus.Pending;
                    accountantSignJournal.DeleteStatus = DeleteStatus.NO;
                    accountantSignJournals.Add(accountantSignJournal);
                }
                else
                {
                    response.AccountantSignRepeat();
                    return response;
                }
            }
            dbContext.AccountantSignJournals.AddRange(accountantSignJournals);
            dbContext.BulkSaveChanges();
            response.Success();            

            return response;
        }

        /// <summary>
        /// 修改印鑑組
        /// </summary>
        /// <param name="accountantSignFormUpdate">印鑑組</param>
        /// <returns></returns>
        public ResponseViewModel UpdateAccountantSigns(List<AccountantSignFormUpdate> accountantSignFormUpdate)
        {
            ResponseViewModel response = new();
            foreach (AccountantSignFormUpdate accountantSignUpdate in accountantSignFormUpdate)
            {
                AccountantSignJournal? accountantSignJournalQuery = dbContext.AccountantSignJournals.Where
                                           (
                                                accountantSignJournal =>
                                                accountantSignJournal.Id == accountantSignUpdate.Id
                                           ).FirstOrDefault();
                if (accountantSignJournalQuery != null)
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = accountantSignUpdate.ImageBase64;
                    mapper.Map(accountantSignUpdate, accountantSignJournalQuery);

                    accountantSignJournalQuery.ImagePath = imagePath;
                    accountantSignJournalQuery.UpdateDate = DateTime.Now;
                    accountantSignJournalQuery.ReviewStatus = Consts.ReviewStatus.Pending;
                }
                else
                {
                    response.DbNoData();                     
                    return response;
                }
            }            
            dbContext.SaveChanges();
            response.Success();            
            return response;
        }

        /// <summary>
        /// 變更此客戶狀態為刪除。
        /// </summary>
        /// <param name="accountantSignId">客戶ID</param>        
        public ResponseViewModel DeleteAccountantSign(int accountantSignId)
        {
            ResponseViewModel response = new();
            AccountantSignJournal? accountantSignJournalQuery = dbContext.AccountantSignJournals
                                .Where(customerSealJournal => customerSealJournal.Id == accountantSignId)
                                .FirstOrDefault();

            if (accountantSignJournalQuery != null)
            {
                accountantSignJournalQuery.DeleteStatus = DeleteStatus.Yes;
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DbNoData();
            }
            return response;
        }

        /// <summary>
        /// 確認會計簽章是否重複 true 不重複 false 重複
        /// </summary>
        /// <param name="accountantSignCheck">查詢參數</param>
        /// <returns></returns>
        private bool CheckRepeatSealMappingConfigId(AccountantSignCheck accountantSignCheck)
        {
            AccountantSignJournal? accountantSignQuery = dbContext.AccountantSignJournals
                                .Where
                                (
                                    customerSealJournal => customerSealJournal.AccountantId == accountantSignCheck.AccountantId
                                    && customerSealJournal.SealMappingConfigId == accountantSignCheck.SealMappingConfigId
                                    && customerSealJournal.DeleteStatus == DeleteStatus.NO
                                ).FirstOrDefault();

            if (accountantSignQuery == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

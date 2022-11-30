using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Util;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用的顧客印鑑組
    /// </summary>
    public class AcoountantSignDeloitteService : IAccountantSignService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;
        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        public AcoountantSignDeloitteService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="accountantId">搜尋條件</param>
        /// <returns></returns>
        public AccountantSignViewModels GetAccountantSings(string accountantId)
        {
            List<AccountantSignViewModel> accountantSignViewModels = new();
            ResponseViewModel response;
            List<AccountantSignJournal> accountantSignJournalQuery = dbContext.AccountantSignJournals.Where
                                                                    (
                                                                        accountantSignJournal => accountantSignJournal.AccountantId == accountantId                                                                        
                                                                    )
                                                                    .Include(accountantSignJournal => accountantSignJournal.SealMappingConfig)
                                                                    .OrderBy(accountantSignJournal => accountantSignJournal.SealMappingConfigId)
                                                                    .ToList();
            if (accountantSignJournalQuery.Any())
            {
                foreach (AccountantSignJournal accountantSignJournal in accountantSignJournalQuery)
                {
                    AccountantSignViewModel customerSealViewModel = mapper.Map<AccountantSignViewModel>(accountantSignJournal);
                    customerSealViewModel.ImageBase64 = "image/..."; //之後會在做BASE64轉換
                    accountantSignViewModels.Add(customerSealViewModel);
                }
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }


            return new AccountantSignViewModels()
            {
                Code = response.Code,
                Message = response.Message,

                SignViewModels = accountantSignViewModels
            };
        }

        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="accountantSignPostDatas">簽名印鑑組</param>
        /// <returns></returns>
        public ResponseViewModel CreateAccountantSigns(List<AccountantSignPost> accountantSignPostDatas)
        {
            ResponseViewModel response = new();
            List<AccountantSignJournal> accountantSignJournals = new();
            foreach (AccountantSignPost accountantSignPostData in accountantSignPostDatas)
            {
                //這段之後會做成IMAGE64的處理並另存在指定的位置
                string imagePath = accountantSignPostData.ImageBase64;

                AccountantSignJournal accountantSignJournal = mapper.Map<AccountantSignJournal>(accountantSignPostData);
                accountantSignJournal.ImagePath = imagePath;
                //customerSealJournal.CreateDate = DateTime.Now;
                accountantSignJournals.Add(accountantSignJournal);
            }
            dbContext.AccountantSignJournals.AddRange(accountantSignJournals);
            dbContext.BulkSaveChanges();
            response = ResponseUtil.Success();

            return response;
        }

        /// <summary>
        /// 修改印鑑組
        /// </summary>
        /// <param name="accountantSignUpdates">印鑑組</param>
        /// <returns></returns>
        public ResponseViewModel UpdateAccountantSigns(List<AccountantSignUpdate> accountantSignUpdates)
        {
            ResponseViewModel response = new();
            foreach (AccountantSignUpdate accountantSignUpdate in accountantSignUpdates)
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
                    accountantSignJournalQuery.ImagePath = imagePath;
                    mapper.Map(accountantSignUpdate, accountantSignJournalQuery);                    
                }
                else
                {
                    response = ResponseUtil.NoData();
                    return response;
                }
            }            
            dbContext.SaveChanges();
            response = ResponseUtil.Success();
            return response;
        }
    }
}

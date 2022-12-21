using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
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
        public AccountantSignCreateDates GetAccountantStartDate(int accountantId)
        {
            AccountantSignCreateDates accountantSignStartDates = new();
            List<AccountantSignCreateDate> signCreateDate = new();
            List<DateTime> accountantSignCreateQuery = dbContext.AccountantSignJournals
                                           .Where
                                           (
                                                accountantSignJournal => accountantSignJournal.AccountantId == accountantId
                                                //&& customerSealJournal.ReviewStatus != ReviewStatus.Pending //待審狀態過濾用
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
            DateTime createNowTime = DateTime.Now;//將建立日期為依據將此次建立的會計師簽印組成為一組Group
            foreach (AccountantSignForm accountantSignPostData in accountantSignPostDatas)
            {
                if(CheckRepeatAccountantSign(mapper.Map<AccountantSignCheck>(accountantSignPostData)))
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = accountantSignPostData.ImageBase64;

                    AccountantSignJournal accountantSignJournal = mapper.Map<AccountantSignJournal>(accountantSignPostData);
                    accountantSignJournal.ImagePath = imagePath;

                    BaseInputAccountantSignJournal(accountantSignJournal, DbActionMode.Create);
                    accountantSignJournal.CreateDate = createNowTime;
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
        /// <param name="accountantSignUpdate">印鑑組</param>
        /// <returns></returns>
        public ResponseViewModel UpdateAccountantSigns(AccountantSignUpdate accountantSignUpdate)
        {
            ResponseViewModel response = new();
            List<AccountantSignJournal> accountantSignJournals = new();

            //新增印鑑
            foreach (AccountantSignForm CreateAccountantSign in accountantSignUpdate.CreateAccountantSigns)
            {
                if (CheckRepeatAccountantSign(mapper.Map<AccountantSignCheck>(CreateAccountantSign))) //確認序號是否重複
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = CreateAccountantSign.ImageBase64;

                    AccountantSignJournal accountantSignJournal = mapper.Map<AccountantSignJournal>(CreateAccountantSign);
                    accountantSignJournal.ImagePath = imagePath;
                    BaseInputAccountantSignJournal(accountantSignJournal, DbActionMode.Create);
                    accountantSignJournals.Add(accountantSignJournal);
                }
                else
                {
                    response.CreateAccountantSignRepeat();
                    return response;
                }
            }
            dbContext.AccountantSignJournals.AddRange(accountantSignJournals);
            dbContext.BulkSaveChanges();
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
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="accountantSignJournal">Db上的印鑑資料</param>
        /// <param name="dbActionMode">對Db所做的行動</param>
        private static void BaseInputAccountantSignJournal(AccountantSignJournal accountantSignJournal, DbActionMode dbActionMode)
        {
            if (DbActionMode.Create == dbActionMode)
            {                
                accountantSignJournal.DeleteStatus = DeleteStatus.NO;
                accountantSignJournal.ReviewStatus = ReviewStatus.Temp;
            }
            else if (DbActionMode.Update == dbActionMode)
            {
                accountantSignJournal.UpdateDate = DateTime.Now;
            }
            accountantSignJournal.StartDate = AvailableDateUtil.NotActivated();
            accountantSignJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            
        }

        /// <summary>
        /// 確認會計簽章是否重複 true 不重複 false 重複
        /// </summary>
        /// <param name="accountantSignCheck">查詢參數</param>
        /// <returns></returns>
        private bool CheckRepeatAccountantSign(AccountantSignCheck accountantSignCheck)
        {
            AccountantSignJournal? accountantSignQuery = dbContext.AccountantSignJournals
                                .FirstOrDefault
                                (
                                    customerSealJournal => customerSealJournal.AccountantId == accountantSignCheck.AccountantId
                                    && customerSealJournal.SealMappingConfigId == accountantSignCheck.SealMappingConfigId
                                    && customerSealJournal.StartDate == accountantSignCheck.StartDate
                                    && customerSealJournal.DeleteStatus == DeleteStatus.NO
                                );

            return accountantSignQuery == null;
        }
    }
}

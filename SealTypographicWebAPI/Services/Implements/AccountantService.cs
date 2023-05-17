using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using DBEntities;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.ComponentModel.Design;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師資料管理
    /// </summary>
    public class AccountantService : IAccountantService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 注入DB與Mapper
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public AccountantService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        ///<inheritdoc />
        public AccountantDetailResponse GetDetail(int accountantId)
        {
            AccountantDetailResponse accountantResponse = new();

            Accountant? accountantQuery = dbContext.Accountants.Include(accountant => accountant.AccountantGroup)
                                                               .FirstOrDefault(accountant => accountant.Id == accountantId);

            if (accountantQuery != null)
            {
                accountantResponse.AccountantDetailViewModel = mapper.Map<AccountantDetailViewModel>(accountantQuery);
            }
            accountantResponse.Success();

            return accountantResponse;
        }

        ///<inheritdoc />
        public AccountantPaginateViewModel GetPaginate(AccountantSearch accountantSearch)
        {            
            return GetStandardPaginate(accountantSearch, false);
        }

        ///<inheritdoc />
        public AccountantPaginateViewModel GetPaginateWithTypographic(AccountantSearch accountantSearch)
        {            
            return GetStandardPaginate(accountantSearch, true);
        }

        ///<inheritdoc />
        public AccountantCreateResponse New(AccountantForm accountantForm)
        {
            AccountantCreateResponse accountantCreateResponse = new();
            int userid = 0;//帳號驗證取得ID
            int companyId = 1;

            //尋找公司並與會計師關聯
            Company? companyQuery = dbContext.Companys.Include(x => x.Accountants).FirstOrDefault(x => x.Id == companyId);

            if (companyQuery != null)
            {
                //驗證編號是否重複
                List<string> accountantCodeQuery = companyQuery.Accountants.Where
                                                (
                                                    x => x.Code == accountantForm.AccountantNumber
                                                    && x.DeleteStatus == DeleteStatus.No
                                                ).Select(x => x.Code).ToList();

                if (!accountantCodeQuery.Any())
                {
                    Accountant dbAccountant = mapper.Map<Accountant>(accountantForm);
                    BaseInputAccountant(dbAccountant, true, userid);
                    companyQuery.Accountants.Add(dbAccountant);
                    dbContext.SaveChanges();

                    if (dbAccountant != null)
                    {
                        //回傳剛建立的會計師基本資料 使建立會計師簽印找到該ID
                        accountantCreateResponse.AccountantId = dbAccountant.Id;
                        accountantCreateResponse.Success();
                    }
                    else
                    {
                        accountantCreateResponse.CreateAccountantFailed();
                    }
                }
                else
                {
                    accountantCreateResponse.AccountantNumberRepeat();
                }
            }


            return accountantCreateResponse;
        }

        ///<inheritdoc />      
        public ResponseViewModel Update(AccountantUpdateForm accountantFormUpdate)
        {
            ResponseViewModel response = new();
            int userid = 0;//帳號驗證取得ID
            Accountant? accountantQuery = dbContext.Accountants.Find(accountantFormUpdate.Id);

            if (accountantQuery != null)
            {
                mapper.Map(accountantFormUpdate, accountantQuery);
                BaseInputAccountant(accountantQuery, false, userid);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.UpdateAccountantNoData();
            }
            return response;
        }

        ///<inheritdoc />     
        public ResponseViewModel Delete(int accountantId)
        {
            ResponseViewModel response = new();
            int userid = 0;//帳號驗證取得ID
            Accountant? accountantQuery = dbContext.Accountants.Find(accountantId);

            if (accountantQuery != null)
            {
                accountantQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputAccountant(accountantQuery, false, userid);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DeleteAccountantNoData();
            }
            return response;
        }

        /// <summary>
        /// 信頭資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="accountant">DB上的客戶資料</param>
        /// <param name="isCreate">確認是否新增的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputAccountant(Accountant accountant, bool isCreate, int userid)
        {
            if (isCreate)
            {
                accountant.CreateUserId = userid;
                accountant.CreateDate = DateTime.Now;
                accountant.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                accountant.UpdateUserId = userid;
                accountant.UpdateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// 依搜尋條件獲得資料列表
        /// </summary>
        /// <param name="accountantSearch">搜尋條件</param>
        /// <param name="isTypographicUse">是否給排版使用</param>
        /// <returns></returns>
        private AccountantPaginateViewModel GetStandardPaginate(AccountantSearch accountantSearch, bool isTypographicUse)
        {
            AccountantPaginateViewModel accountantPaginatesViewModels = new();
            int companyId = 1;

            IQueryable<Accountant> accountantQuery = dbContext.Accountants.Where
                                                        (
                                                            accountant => accountant.Company.Id == companyId
                                                            && accountant.DeleteStatus == DeleteStatus.No
                                                        );

            if (!string.IsNullOrWhiteSpace(accountantSearch.KeyWord))
            {
                accountantQuery = accountantQuery.Where
                (
                    accountant =>
                    accountant.Code.ToLower().Contains(accountantSearch.KeyWord.ToLower())
                    || accountant.Name.Contains(accountantSearch.KeyWord)
                );
                
                if (accountantSearch.AccountantGroupName != null)
                {
                    accountantQuery = accountantQuery.Where(accountant => accountant.AccountantGroup.Name.Contains(accountantSearch.KeyWord));

                }
            }

            accountantQuery = accountantQuery.OrderBy(accountant => accountant.Id);
            if (accountantQuery.Any())
            {
                //取得該頁            
                List<Accountant> thisPageAccountants = accountantQuery
                                          .Include(accountant => accountant.AccountantSignGroupJournals)
                                          .Include(accountant => accountant.AccountantGroup)
                                          .Skip((accountantSearch.PageNumber - 1) * accountantSearch.PageSize)
                                          .Take(accountantSearch.PageSize)
                                          .ToList();


                foreach (Accountant accountant in thisPageAccountants)
                {
                    AccountantViewModelWithCreateDate accountantPaginatesViewModel = mapper.Map<AccountantViewModelWithCreateDate>(accountant);


                    if (accountant.AccountantSignGroupJournals.Any())
                    {                        
                        if (isTypographicUse)
                        {
                            accountantPaginatesViewModel.AccountantSignGroupId = accountant.AccountantSignGroupJournals
                                                                                .Where
                                                                                (
                                                                                    x => x.DeleteStatus == DeleteStatus.No
                                                                                    && x.ReviewStatus == ReviewStatus.Approval
                                                                                )
                                                                                .OrderByDescending(x => x.CreateDate)
                                                                                .Select(x => x.Id)
                                                                                .FirstOrDefault();
                        }
                        else
                        {
                            accountantPaginatesViewModel.AccountantSignGroupId = accountant.AccountantSignGroupJournals
                                                                                 .Where
                                                                                 (
                                                                                        x => x.DeleteStatus == DeleteStatus.No
                                                                                        && x.ReviewStatus < ReviewStatus.Disabled
                                                                                 )
                                                                                 .OrderByDescending(x => x.CreateDate)
                                                                                 .Select(x => x.Id)
                                                                                 .FirstOrDefault();
                        }

                        if(accountantPaginatesViewModel.AccountantSignGroupId > 0)
                        {
                            accountantPaginatesViewModels.ViewModels.Add(accountantPaginatesViewModel);
                        }
                        
                    }                    
                }
                accountantPaginatesViewModels.PageNumber = accountantSearch.PageNumber;
                accountantPaginatesViewModels.PageSize = accountantSearch.PageSize;
                //計算總頁數
                accountantPaginatesViewModels.TotalPage = TotalPageUtil.GetTotalPage(accountantQuery.Count(), accountantSearch.PageSize);
                accountantPaginatesViewModels.TotalCount = accountantQuery.Count();
            }
            accountantPaginatesViewModels.Success();

            return accountantPaginatesViewModels;
        }

       
    }
}
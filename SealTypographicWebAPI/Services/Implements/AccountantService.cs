using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Linq;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用 管理會計師資料
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
        public AccountantService(SealTypographicDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得會計師資料
        /// </summary>
        /// <param name="accountantId"></param>
        /// <returns></returns>
        public AccountantResponse GetAccountant(int accountantId)
        {
            AccountantResponse accountantResponse = new();
            ResponseViewModel response = new();
            Accountant? accountantQuery = dbContext.Accountants.Where(accountant => accountant.Id == accountantId)
                                                                .Include(accountant => accountant.AccountantGroup)
                                                                .FirstOrDefault();

            if (accountantQuery != null)
            {
                accountantResponse.AccountantViewModel = mapper.Map<AccountantViewModel>(accountantQuery);
                accountantResponse.Success();
            }
            else
            {
                accountantResponse.DbNoData();
            }
            return accountantResponse;
        }

        /// <summary>
        /// 依搜尋條件獲得會計資料列表
        /// </summary>
        /// <param name="accountantSearch">會計師分頁搜尋</param> 
        /// <returns></returns>
        public AccountantPaginatesViewModel GetAccountantViewModels(AccountantSearch accountantSearch)
        {
            AccountantPaginatesViewModel accountantPaginatesViewModel = new();
            List<AccountantViewModel> accountantViewModels = new();
            ResponseViewModel response = new();
            IQueryable<Accountant> accountantQuery = dbContext.Accountants;
            if (!string.IsNullOrWhiteSpace(accountantSearch.NumberOrNameOrGroupsName))
            {
                accountantQuery = accountantQuery.Where
                                                    (
                                                        accountant =>
                                                        accountant.AccountantNumber.ToLower().Contains(accountantSearch.NumberOrNameOrGroupsName.ToLower())                                                         
                                                        || accountant.Name.Contains(accountantSearch.NumberOrNameOrGroupsName)
                                                        || accountant.AccountantGroup.Name.Contains(accountantSearch.NumberOrNameOrGroupsName)
                                                    );                                                   
            }

            accountantQuery = accountantQuery.OrderBy(accountant => accountant.Id);
            if (accountantQuery.Any())
            {
                //取得該頁            
                List<Accountant> thisPageAccountants = accountantQuery
                                          .Include(accountantGroup => accountantGroup.AccountantGroup)
                                          .Skip((accountantSearch.PageNumber - 1) * accountantSearch.PageSize)
                                          .Take(accountantSearch.PageSize)
                                          .ToList();
                foreach (Accountant accountant in thisPageAccountants)
                {
                    AccountantViewModel accountantViewModel = mapper.Map<AccountantViewModel>(accountant);
                    accountantViewModels.Add(accountantViewModel);
                }
                accountantPaginatesViewModel.AccountantViewModels = accountantViewModels;
                accountantPaginatesViewModel.PageNumber= accountantSearch.PageNumber;
                accountantPaginatesViewModel.PageSize = accountantSearch.PageSize;
                //計算總頁數
                accountantPaginatesViewModel.TotalPage = TotalPageUtil.GetTotalPage(accountantQuery.Count(), accountantSearch.PageSize);               
                accountantPaginatesViewModel.TotalCount = accountantQuery.Count();
                accountantPaginatesViewModel.Success();
            }
            else
            {
                accountantPaginatesViewModel.DbNoData();                
            }
            return accountantPaginatesViewModel;
        }

        /// <summary>
        /// 新增會計基本資料
        /// </summary>
        /// <param name="accountantForm">基本資料</param>
        /// <returns></returns>
        public ResponseViewModel CreateAccountant(AccountantForm accountantForm)
        {
            ResponseViewModel response = new();
            Accountant? accountantQuery = dbContext.Accountants
                                    .Where(accountant => accountant.AccountantNumber == accountantForm.AccountantNumber)
                                    .FirstOrDefault();

            if (accountantQuery == null)
            {
                Accountant accountant = mapper.Map<Accountant>(accountantForm);
                accountant.CreateDate = DateTime.Now;
                accountant.DeleteStatus = DeleteStatus.NO;
                dbContext.Accountants.Add(accountant);
                dbContext.SaveChanges();
                response.Success();                
            }
            else
            {
                response.AccountantNumberRepeat();                
            }
            return response;
        }

        /// <summary>
        /// 更新會計師基本資料
        /// </summary>
        /// <param name="accountantFormUpdate">會計師基本資料 accountantBaseData.id 為搜尋條件</param>        
        public ResponseViewModel UpdateAccountant(AccountantFormUpdate accountantFormUpdate)
        {
            ResponseViewModel response = new();
            Accountant? accountantQuery = dbContext.Accountants.Find(accountantFormUpdate.Id);

            if (accountantQuery != null)
            {
                mapper.Map(accountantFormUpdate, accountantQuery);
                accountantQuery.UpdateDate = DateTime.Now;
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
        /// 變更此客戶狀態為刪除(隱藏)。
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        public ResponseViewModel DeleteAccountant(int accountantId)
        {
            ResponseViewModel response = new();
            Accountant? accountantQuery = dbContext.Accountants.Find(accountantId);

            if (accountantQuery != null)
            {
                accountantQuery.DeleteStatus = DeleteStatus.Yes;
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DbNoData();
            }
            return response;
        }

    }
}
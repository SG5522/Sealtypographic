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
            List<AccountantViewModel> accountantViewModels = new();
            ResponseViewModel response = new();
            IQueryable<Accountant> accountantsQuery = dbContext.Accountants;
            int totalPage = 0;
            int totalCount = 0;
            if (!string.IsNullOrWhiteSpace(accountantSearch.IdOrNameOrGroupsName))
            {
                accountantsQuery = accountantsQuery.Where
                                                    (
                                                        accountant =>
                                                        accountant.AccountantNumber.ToLower().Contains(accountantSearch.IdOrNameOrGroupsName.ToLower())                                                         
                                                        || accountant.Name.Contains(accountantSearch.IdOrNameOrGroupsName)
                                                        || accountant.AccountantGroup.Name.Contains(accountantSearch.IdOrNameOrGroupsName)
                                                    );                                                   
            }

            accountantsQuery = accountantsQuery.OrderBy(accountant => accountant.Id);
            if (accountantsQuery.Any())
            {
                //取得該頁            
                List<Accountant> thisPageAccountants = accountantsQuery
                                          .Include(accountantGroup => accountantGroup.AccountantGroup)
                                          .Skip((accountantSearch.PageNumber - 1) * accountantSearch.PageSize)
                                          .Take(accountantSearch.PageSize)
                                          .ToList();
                //計算總頁數
                totalPage = accountantsQuery.Count() / accountantSearch.PageSize + (accountantsQuery.Count() % accountantSearch.PageSize == 0 ? 0 : 1);
                totalCount = accountantsQuery.Count();
                foreach (Accountant accountant in thisPageAccountants)
                {
                    AccountantViewModel accountantViewModel = mapper.Map<AccountantViewModel>(accountant);
                    accountantViewModels.Add(accountantViewModel);
                }
                //取得成功訊息
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
            }

            return new()
            {
                PageNumber = accountantSearch.PageNumber,
                TotalCount = totalCount,
                TotalPage = totalPage,
                AccountantViewModels = accountantViewModels,
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message,
            };
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
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.UniqueConstraintFailed();
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
            Accountant? accountantQuery = dbContext.Accountants
                                .Where(accountant => accountant.AccountantNumber == accountantFormUpdate.AccountantNumber)
                                .FirstOrDefault();

            if (accountantQuery != null)
            {
                mapper.Map(accountantFormUpdate, accountantQuery);
                accountantQuery.UpdateDate = DateTime.Now;
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
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
            Accountant? accountantQuery = dbContext.Accountants
                                .Where(accountant => accountant.Id == accountantId)
                                .FirstOrDefault();

            if (accountantQuery != null)
            {
                accountantQuery.DeleteStatus = DeleteStatus.Yes;
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
            }
            return response;
        }

    }
}
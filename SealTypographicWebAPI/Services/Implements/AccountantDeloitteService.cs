using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用 管理會計師資料
    /// </summary>
    public class AccountantDeloitteService : IAccountantService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 注入DB與Mapper
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public AccountantDeloitteService(SealTypographicDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得會計師資料
        /// </summary>
        /// <param name="accountantId"></param>
        /// <returns></returns>
        public AccountantResponse GetAccountant(string accountantId)
        {
            AccountantViewModel accountantViewModel = new();
            ResponseViewModel response = new();
            Accountant? accountantQuery = dbContext.Accountants.Where(accountant => accountant.Id == accountantId)
                                                                .Include(accountant => accountant.AccountantGroup)                                                                
                                                                .FirstOrDefault();

            if (accountantQuery != null)
            {
                accountantViewModel = mapper.Map<AccountantViewModel>(accountantQuery);                
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return new AccountantResponse()
            {
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message,

                AccountantViewModel = accountantViewModel
            };
        }

        /// <summary>
        /// 依搜尋條件獲得會計資料列表
        /// </summary>
        /// <param name="accountantSearch">會計師分頁搜尋</param> 
        /// <returns></returns>
        public AccountantPaginatesViewModel GetAccountantViewModels(AccountantSearch accountantSearch)
        {            
            List<AccountantPaginateViewModel> accountantPaginateViewModels = new();
            ResponseViewModel response = new();
            IQueryable<Accountant> accountantsQuery = dbContext.Accountants;
            int totalPage = 0;
            int totalCount = 0;            
            if (!string.IsNullOrWhiteSpace(accountantSearch.IdOrNameOrGroupsName))
            {
                accountantsQuery = accountantsQuery.Where
                                                    (
                                                        accountant =>
                                                        accountant.Id.Contains(accountantSearch.IdOrNameOrGroupsName)
                                                        || accountant.Name.Contains(accountantSearch.IdOrNameOrGroupsName)
                                                        || accountant.AccountantGroup.Name.Contains(accountantSearch.IdOrNameOrGroupsName)
                                                    );                                                   
            }
            if(accountantSearch.Status != (int)Status.All)
            {
                accountantsQuery = accountantsQuery.Where(accountant => accountant.Status == accountantSearch.Status);
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
                    AccountantPaginateViewModel accountantPaginateViewModel = mapper.Map<AccountantPaginateViewModel>(accountant);
                    accountantPaginateViewModel.StatusString = StatusUtil.Get((Status)accountant.Status);
                    accountantPaginateViewModels.Add(accountantPaginateViewModel);
                }
                //取得成功訊息
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return new()
            {
                PageNumber = accountantSearch.PageNumber,
                TotalCount = totalCount,
                TotalPage = totalPage,
                AccountantPaginates = accountantPaginateViewModels,
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message,
            };
        }

        /// <summary>
        /// 新增會計基本資料
        /// </summary>
        /// <param name="accountantPostData">基本資料</param>
        /// <returns></returns>
        public ResponseViewModel CreateAccountant(AccountantForm accountantPostData)
        {
            ResponseViewModel response = new();
            Accountant? accountantQuery = dbContext.Accountants
                                    .Where(accountant => accountant.Id == accountantPostData.Id)
                                    .FirstOrDefault();

            if (accountantQuery == null)
            {
                Accountant accountant = mapper.Map<Accountant>(accountantPostData);
                accountant.CreateDate = DateTime.Now;
                accountant.AvailableDate = AvailableDateUtil.NotActivated();
                accountant.Status = (int)Status.Pending;

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
        /// <param name="accountaPostData">會計師基本資料 accountantBaseData.id 為搜尋條件</param>        
        public ResponseViewModel UpdateAccountant(AccountantForm accountaPostData)
        {
            ResponseViewModel response = new();
            Accountant? accountantQuery = dbContext.Accountants
                                .Where(accountant => accountant.Id == accountaPostData.Id)
                                .FirstOrDefault();

            if (accountantQuery != null)
            {
                mapper.Map(accountaPostData, accountantQuery);
                accountantQuery.AvailableDate = AvailableDateUtil.NotActivated();
                accountantQuery.Status = (int)Status.Pending;

                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return response;
        }

        /// <summary>
        /// 變更此客戶狀態為刪除(隱藏)。
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        public ResponseViewModel DeleteAccountant(string accountantId)
        {
            ResponseViewModel response = new();
            Accountant? accountantQuery = dbContext.Accountants
                                .Where(accountant => accountant.Id == accountantId)
                                .FirstOrDefault();

            if (accountantQuery != null)
            {                
                accountantQuery.AvailableDate = AvailableDateUtil.NotActivated();
                accountantQuery.Status = (int)Status.Hidden;
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }
            return response;
        }

    }
}
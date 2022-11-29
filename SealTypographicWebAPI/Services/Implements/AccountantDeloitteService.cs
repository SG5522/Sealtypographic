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
            Response response = new();
            Accountant? accountantQuery = dbContext.Accountants.Where(accountant => accountant.Id == accountantId)
                                                                .Include(accountant => accountant.AccountantGroup)                                                                
                                                                .FirstOrDefault();

            if (accountantQuery != null)
            {
                accountantViewModel = mapper.Map<AccountantViewModel>(accountantQuery);
                accountantViewModel.StatusString = StatusUtil.Get((Status)accountantQuery.Status);
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
        /// <param name="accountantQueryPage">會計師分頁搜尋</param> 
        /// <returns></returns>
        public AccountantResponses GetAccountantViewModels(AccountantQueryPage accountantQueryPage)
        {
            List<AccountantViewModel> accountantViewModels = new();
            Response response = new();
            IQueryable<Accountant> accountantsQuery = dbContext.Accountants;
            int totalPage = 0;
            int totalCount = 0;
            
            if (!string.IsNullOrWhiteSpace(accountantQueryPage.IdOrNameOrGroupsName))
            {
                accountantsQuery = accountantsQuery.Where
                                                    (
                                                        accountant =>
                                                        accountant.Id.Contains(accountantQueryPage.IdOrNameOrGroupsName)
                                                        || accountant.Name.Contains(accountantQueryPage.IdOrNameOrGroupsName)
                                                        || accountant.AccountantGroup.Name.Contains(accountantQueryPage.IdOrNameOrGroupsName)
                                                    );                                                   
            }
            accountantsQuery = accountantsQuery.OrderBy(accountant => accountant.Id);
            if (accountantsQuery.Any())
            {
                //取得該頁            
                List<Accountant> thisPageAccountants = accountantsQuery
                                          .Include(accountantGroup => accountantGroup.AccountantGroup)
                                          .Skip((accountantQueryPage.PageNumber - 1) * accountantQueryPage.PageSize)
                                          .Take(accountantQueryPage.PageSize)
                                          .ToList();
                //計算總頁數
                totalPage = accountantsQuery.Count() / accountantQueryPage.PageSize + (accountantsQuery.Count() % accountantQueryPage.PageSize == 0 ? 0 : 1);
                totalCount = accountantsQuery.Count();
                foreach (Accountant accountant in thisPageAccountants)
                {
                    AccountantViewModel accountantViewModel = mapper.Map<AccountantViewModel>(accountant);
                    accountantViewModel.StatusString = StatusUtil.Get((Status)accountant.Status);
                    accountantViewModels.Add(accountantViewModel);
                }
                //取得成功訊息
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return new AccountantResponses()
            {
                PageNumber = accountantQueryPage.PageNumber,
                TotalCount = totalCount,
                TotalPage = totalPage,
                Accountants = accountantViewModels,
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
        public Response CreateAccountant(AccountantPostData accountantPostData)
        {
            Response response = new();
            Accountant? accountantQuery = dbContext.Accountants
                                    .Where(accountant => accountant.Id == accountantPostData.Id)
                                    .FirstOrDefault();

            if (accountantQuery == null)
            {
                Accountant accountant = mapper.Map<Accountant>(accountantPostData);
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
        public Response UpdateAccountant(AccountantPostData accountaPostData)
        {
            Response response = new();
            Accountant? accountantQuery = dbContext.Accountants
                                .Where(accountant => accountant.Id == accountaPostData.Id)
                                .FirstOrDefault();

            if (accountantQuery != null)
            {
                mapper.Map(accountaPostData, accountantQuery);

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
        public Response DeleteAccountant(string accountantId)
        {
            Response response = new();
            var accountantQuery = dbContext.Accountants
                                .Where(accountant => accountant.Id == accountantId);

            if (accountantQuery.Any())
            {
                var accountant = accountantQuery.First();
                accountant.Status = 2;
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
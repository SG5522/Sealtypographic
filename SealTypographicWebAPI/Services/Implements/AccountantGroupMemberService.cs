using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Models.AccountantGroupMember;
using SealTypographicWebAPI.Models.Accountant;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Utils;
using SealTypographicWebAPI.Models.Customer;
using System.Linq;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用 管理會計群組成員
    /// </summary>
    public class AccountantGroupMemberService : IAccountantGroupMemberService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>                
        public AccountantGroupMemberService(SealTypographicDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得會計群組的會計師列表
        /// </summary>
        /// <param name="accountantGroupMemberSearch">會計群組搜尋條件(分頁)</param>
        /// <returns></returns>
        public AccountantGroupMembers GetAccountantGroupMembers(AccountantGroupMemberSearch accountantGroupMemberSearch)
        {
            AccountantGroupMembers accountantGroupMembers = new();
            List<AccountantGroupMember> accountantMembers = new();
            int totalPage = 0;
            IQueryable<Accountant> accountantQuery = dbContext.Accountants
                                           .Where(accountant => accountant.AccountantGroupId == accountantGroupMemberSearch.AccountantGroupId)
                                           .OrderBy(accountant => accountant.Id);

            if (accountantQuery.Any())
            {
                //取得該頁            
                List<Accountant> accountants = accountantQuery
                                            .Skip((accountantGroupMemberSearch.PageNumber - 1) * accountantGroupMemberSearch.PageSize)
                                            .Take(accountantGroupMemberSearch.PageSize)
                                            .ToList();
                //計算總頁數
                totalPage = TotalPageUtil.GetTotalPage(accountantQuery.Count(), accountantGroupMemberSearch.PageSize);                
                foreach (Accountant accountant in accountants)
                {
                    accountantMembers.Add(new()
                    {
                        Id = accountant.Id,
                        AccountantNumber = accountant.AccountantNumber,
                        Name = accountant.Name,
                    });
                }
                accountantGroupMembers.Members = accountantMembers;                
                accountantGroupMembers.Success();               
            }
            else
            {
                accountantGroupMembers.DbNoData();                
            }
            accountantGroupMembers.TotalPage = totalPage;
            accountantGroupMembers.TotalCount = accountantQuery.Count();

            return accountantGroupMembers;
        }

        /// <summary>
        /// 取得非此群組的成員
        /// </summary>
        /// <param name="notThisGroupMemberSearch">搜尋條件</param>
        public NotThisGroupMember GetNotThisGroupMember(NotThisGroupMemberSearch notThisGroupMemberSearch)
        {
            List<AccountantViewModel> accountantViewModels = new();
            ResponseViewModel response;
            int totalPage = 0;
            int totalCount = 0;
            IQueryable<Accountant>? accountantQuery = dbContext.Accountants
                                                .Where(accountant => accountant.AccountantGroupId != notThisGroupMemberSearch.AccountantGroupId)
                                                .Include(accountant => accountant.AccountantGroup);
                                                
            if(notThisGroupMemberSearch.AccountantNumberOrName != null)
            {
                accountantQuery = accountantQuery.Where
                                (
                                    accountant => accountant.AccountantNumber.Contains(notThisGroupMemberSearch.AccountantNumberOrName)
                                    || accountant.Name.Contains(notThisGroupMemberSearch.AccountantNumberOrName)
                                );
            }

            if (accountantQuery.Any())
            {
                //取得該頁            
                List<Accountant> accountants =  accountantQuery
                                                .Skip((notThisGroupMemberSearch.PageNumber - 1) * notThisGroupMemberSearch.PageSize)
                                                .Take(notThisGroupMemberSearch.PageSize)
                                                .ToList();
                //計算總頁數
                totalPage = accountantQuery.Count() / notThisGroupMemberSearch.PageSize + (accountantQuery.Count() % notThisGroupMemberSearch.PageSize == 0 ? 0 : 1);
                totalCount = accountantQuery.Count();

                foreach (Accountant accountant in accountants)
                {
                    AccountantViewModel accountantViewModel = mapper.Map<AccountantViewModel>(accountant);
                    accountantViewModel.AccountantGroupName = accountant.AccountantGroup.Name;
                    accountantViewModels.Add(accountantViewModel);
                }
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
            }

            return new()
            {
                PageNumber = notThisGroupMemberSearch.PageNumber,
                PageSize = notThisGroupMemberSearch.PageSize,
                TotalCount = totalPage,
                TotalPage = totalPage,
                AccountantGroupid = notThisGroupMemberSearch.AccountantGroupId,
                AccountantViewModels = accountantViewModels,
                
                //回應
                Code = response.Code,
                Message= response.Message,
            };
        }

        /// <summary>
        /// 變更會計師群組(單個)
        /// </summary>
        /// <param name="accountantGroupChangeForm"></param>
        /// <returns></returns>
        public ResponseViewModel UpdateAccountantGroup(AccountantGroupChangeForm accountantGroupChangeForm)
        {
            ResponseViewModel response;
            Accountant? accountant = dbContext.Accountants
                                    .Where(accountant => accountant.Id == accountantGroupChangeForm.Id)
                                    .FirstOrDefault();
            if (accountant != null)
            {
                accountant.AccountantGroupId = accountantGroupChangeForm.AccountantGroupId;
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
        /// 變更多個會計師的群組
        /// </summary>
        /// <param name="accountantGroupMemberForm"></param>
        /// <returns></returns>
        public ResponseViewModel ChangeNotTheGroupMember(AccountantGroupMemberForm accountantGroupMemberForm)
        {
            ResponseViewModel response;
            IQueryable<Accountant> accountants = dbContext.Accountants;
            foreach (int AccountantId in accountantGroupMemberForm.AccountantIds)
            {
                Accountant? accountantQuery = accountants
                                    .Where(accountant => accountant.Id == AccountantId)
                                    .FirstOrDefault();

                if (accountantQuery != null)
                {
                    accountantQuery.AccountantGroupId = accountantGroupMemberForm.AccountantGroupId;
                }                
                else
                {
                    response = ResponseUtil.UniqueConstraintFailed();
                    return response;
                }
            }
            dbContext.SaveChanges();
            response = ResponseUtil.Success();

            return response;
        }
    }
}

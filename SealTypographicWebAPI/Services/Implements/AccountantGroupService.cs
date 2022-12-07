using EFCore.BulkExtensions;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Util;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用 管理會計師群組
    /// </summary>
    public class AccountantGroupService : IAccountantGroupService
    {
        private readonly SealTypographicDbContext dbContext;        

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        public AccountantGroupService(SealTypographicDbContext dbContext)
        {
            this.dbContext = dbContext;            
        }        

        /// <summary>
        /// 取得會計師群組所有資料
        /// </summary>
        /// <returns></returns>
        public AccountantGroupList GetAccountantGroupList()
        {
            ResponseViewModel response;
            List<AccountantGroupData> accountantGroupDatas = new();            

            List<AccountantGroup> accountantGroups = dbContext.AccountantGroups.ToList();
            if (accountantGroups.Any())
            {
                foreach (AccountantGroup accountantGroup in accountantGroups)
                {
                    accountantGroupDatas.Add(new()
                    {
                        Id = accountantGroup.Id,
                        Name = accountantGroup.Name,
                    });
                }
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return new()
            {
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message,

                AccountantGroupDatas = accountantGroupDatas
            };
        }

        /// <summary>
        /// 取得會計師群組資料
        /// </summary>
        /// <param name="accountantGroupId">群組ID</param>
        /// <returns></returns>
        public AccountantGroupResponse GetAccountantGroupData(int accountantGroupId)
        {
            AccountantGroupData accountantGroupData = new();
            ResponseViewModel response;
            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                    .Where(accountantGroup => accountantGroup.Id == accountantGroupId)
                                                    .FirstOrDefault();

            if (accountantGroupQuery != null)
            {                
                accountantGroupData.Id = accountantGroupQuery.Id;
                accountantGroupData.Name = accountantGroupQuery.Name;

                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return new AccountantGroupResponse()
            {
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message,

                AccountantGroupData = accountantGroupData
            };
        }

        /// <summary>
        /// 會計師群組分頁搜尋
        /// </summary>
        /// <param name="accountantGroupQueryPage">accountantGroupData</param>
        /// <returns></returns>
        public AccountantGroupResponses GetAccountantGroups(AccountantGroupSearch accountantGroupQueryPage)
        {
            List<AccountantGroupData> accountantGroups = new();
            ResponseViewModel response;
            int totalPage = 0;
            int totalCount = 0;
            IQueryable<AccountantGroup> accountantGroupsQuery = dbContext.AccountantGroups;
            if (!string.IsNullOrWhiteSpace(accountantGroupQueryPage.GroupName))
            {
                accountantGroupsQuery = accountantGroupsQuery.Where
                                        (
                                            accountantGroup =>                                            
                                            accountantGroup.Name.Contains(accountantGroupQueryPage.GroupName)
                                        );
            }
            accountantGroupsQuery.OrderBy(accountantGroup => accountantGroup.Id);

            if (accountantGroupsQuery.Any())
            {
                //取得該頁            
                List<AccountantGroup> thisPageAccountantGroups = accountantGroupsQuery
                                                                .Skip((accountantGroupQueryPage.PageNumber - 1) * accountantGroupQueryPage.PageSize)
                                                                .Take(accountantGroupQueryPage.PageSize)
                                                                .ToList();
                //計算總頁數
                totalPage = accountantGroupsQuery.Count() / accountantGroupQueryPage.PageSize + (accountantGroupsQuery.Count() % accountantGroupQueryPage.PageSize == 0 ? 0 : 1);
                totalCount = accountantGroupsQuery.Count();
                foreach (var accountantGroup in thisPageAccountantGroups)
                {
                    accountantGroups.Add(new AccountantGroupData()
                    {
                        Id = accountantGroup.Id,
                        Name = accountantGroup.Name,
                    });
                }
                //取得成功訊息
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return new AccountantGroupResponses()
            {
                PageNumber = accountantGroupQueryPage.PageNumber,
                PageSize = accountantGroupQueryPage.PageSize,
                TotalCount = totalCount,
                TotalPage = totalPage,
                AccountantGroups = accountantGroups,
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message,
            };
        }       

        /// <summary>
        /// 建立會計師群組資料
        /// </summary>
        /// <param name="accountantGroupData">群組資料</param>        
        public ResponseViewModel CreateAccountantGroup(AccountantGroupData accountantGroupData)
        {
            ResponseViewModel response;
            IQueryable<AccountantGroup> accountantGroupQuery = dbContext.AccountantGroups
                                                                .Where(accountantGroup => accountantGroup.Id == accountantGroupData.Id);

            if (!accountantGroupQuery.Any())
            {
                AccountantGroup accountantGroup = new()
                {
                    Id = accountantGroupData.Id,
                    Name = accountantGroupData.Name,
                };
                dbContext.AccountantGroups.Add(accountantGroup);
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
        /// 更新會計師群組資料
        /// </summary>
        /// <param name="accountantGroupData">群組資料</param>
        public ResponseViewModel UpdateAccountantGroup(AccountantGroupData accountantGroupData)
        {
            ResponseViewModel response;
            IQueryable<AccountantGroup> accountantGroupQuery = dbContext.AccountantGroups
                                                                .Where(accountantGroup => accountantGroup.Id == accountantGroupData.Id);

            if (accountantGroupQuery.Any())
            {
                AccountantGroup accountantGroup = accountantGroupQuery.First();
                accountantGroup.Id = accountantGroupData.Id;
                accountantGroup.Name = accountantGroupData.Name;
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
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupId"></param>
        public ResponseViewModel DeleteAccountantGroup(int accountantGroupId)
        {
            ResponseViewModel response;
            IQueryable<AccountantGroup> accountantGroupQuery = dbContext.AccountantGroups.Where
                                                               (
                                                                    accountantGroup =>
                                                                    accountantGroup.Id == accountantGroupId
                                                               );
            if (accountantGroupQuery.Any())
            {
                dbContext.Accountants.Where
                (
                        accountant =>
                        accountant.Id == accountantGroupId
                ).BatchUpdate(new Accountant { AccountantGroupId = "0" });

                AccountantGroup accountantGroup = accountantGroupQuery.First();
                dbContext.AccountantGroups.Remove(accountantGroup);
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

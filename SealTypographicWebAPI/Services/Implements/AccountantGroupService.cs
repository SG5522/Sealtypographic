using AutoMapper;
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
        private readonly IMapper mapper;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>        
        public AccountantGroupService(SealTypographicDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
        }        

        /// <summary>
        /// 取得會計師群組所有資料
        /// </summary>
        /// <returns></returns>
        public AccountantGroupList GetAccountantGroupList()
        {
            ResponseViewModel response;
            List<AccountantGroupViewModel> accountantGroupDatas = new();            

            List<AccountantGroup> accountantGroups = dbContext.AccountantGroups.ToList();
            if (accountantGroups.Any())
            {
                foreach (AccountantGroup accountantGroup in accountantGroups)
                {
                    accountantGroupDatas.Add(mapper.Map<AccountantGroupViewModel>(accountantGroup));
                    //accountantGroupDatas.Add(new()
                    //{
                    //    Id = accountantGroup.Id,
                    //    Name = accountantGroup.Name,
                    //});
                }
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
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
            AccountantGroupViewModel accountantGroupData = new();
            ResponseViewModel response;
            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                    .Where(accountantGroup => accountantGroup.Id == accountantGroupId)
                                                    .FirstOrDefault();

            if (accountantGroupQuery != null)
            {
                accountantGroupData = mapper.Map<AccountantGroupViewModel>(accountantGroupQuery);
                //accountantGroupData.Id = accountantGroupQuery.Id;
                //accountantGroupData.Name = accountantGroupQuery.Name;

                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
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
            List<AccountantGroupViewModel> accountantGroups = new();
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
                foreach (AccountantGroup accountantGroup in thisPageAccountantGroups)
                {
                    accountantGroups.Add(mapper.Map<AccountantGroupViewModel>(accountantGroup));
                    //accountantGroups.Add(new AccountantGroupViewModel()
                    //{
                    //    Id = accountantGroup.Id,
                    //    Name = accountantGroup.Name,
                    //});
                }
                //取得成功訊息
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
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
        /// <param name="accountantGroupForm">群組資料</param>        
        public ResponseViewModel CreateAccountantGroup(AccountantGroupForm accountantGroupForm)
        {
            ResponseViewModel response;
            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                .Where(accountantGroup => accountantGroup.AccountantGroupNumber == accountantGroupForm.AccountantGroupNumber)
                                                .FirstOrDefault();

            if (accountantGroupQuery == null)
            {
                AccountantGroup accountantGroup = mapper.Map<AccountantGroup>(accountantGroupForm);
                accountantGroup.CreateDate = DateTime.Now;
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
    /// <param name="accountantGroupFormUpdate">群組資料</param>
    public ResponseViewModel UpdateAccountantGroup(AccountantGroupFormUpdate accountantGroupFormUpdate)
        {
            ResponseViewModel response;
            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                    .Where(accountantGroup => accountantGroup.Id == accountantGroupFormUpdate.Id)
                                                    .FirstOrDefault();
            if (accountantGroupQuery != null)
            {
                mapper.Map(accountantGroupFormUpdate, accountantGroupQuery);
                accountantGroupQuery.UpdateDate = DateTime.Now;
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
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupId">群組Id</param>
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
                ).BatchUpdate(new Accountant { AccountantGroupId = 1 });

                AccountantGroup accountantGroup = accountantGroupQuery.First();
                dbContext.AccountantGroups.Remove(accountantGroup);
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

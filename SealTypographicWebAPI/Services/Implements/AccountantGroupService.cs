using AutoMapper;
using EFCore.BulkExtensions;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Utils;

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
            AccountantGroupList accountantGroupList = new();
            List<AccountantGroupViewModel> accountantGroupDatas = new();            

            List<AccountantGroup> accountantGroups = dbContext.AccountantGroups
                                                    .Where(accountantGroup => accountantGroup.DeleteStatus == DeleteStatus.NO)
                                                    .ToList();
            if (accountantGroups.Any())
            {
                foreach (AccountantGroup accountantGroup in accountantGroups)
                {
                    accountantGroupDatas.Add(mapper.Map<AccountantGroupViewModel>(accountantGroup));
                }
                accountantGroupList.AccountantGroupDatas = accountantGroupDatas;
                accountantGroupList.Success();
                
            }
            else
            {
                accountantGroupList.AccountantGroupNoData();
            }

            return accountantGroupList;
        }

        /// <summary>
        /// 取得會計師群組資料
        /// </summary>
        /// <param name="accountantGroupId">群組ID</param>
        /// <returns></returns>
        public AccountantGroupResponse GetAccountantGroupData(int accountantGroupId)
        {
            AccountantGroupResponse accountantGroupResponse = new();
            AccountantGroupViewModel accountantGroupData = new();
            
            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                    .Where(accountantGroup => accountantGroup.Id == accountantGroupId)
                                                    .FirstOrDefault();

            if (accountantGroupQuery != null)
            {
                accountantGroupData = mapper.Map<AccountantGroupViewModel>(accountantGroupQuery);
                accountantGroupResponse.AccountantGroupData = accountantGroupData;
                accountantGroupResponse.Success();                
            }
            else
            {
                accountantGroupResponse.AccountantGroupNoData();
            }

            return accountantGroupResponse;
        }

        /// <summary>
        /// 會計師群組分頁搜尋
        /// </summary>
        /// <param name="accountantGroupQueryPage">accountantGroupData</param>
        /// <returns></returns>
        public AccountantGroupResponses GetAccountantGroups(AccountantGroupSearch accountantGroupQueryPage)
        {
            AccountantGroupResponses accountantGroupResponses = new();
            List<AccountantGroupViewModel> accountantGroups = new();
            
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

                foreach (AccountantGroup accountantGroup in thisPageAccountantGroups)
                {
                    accountantGroups.Add(mapper.Map<AccountantGroupViewModel>(accountantGroup));
                }
                accountantGroupResponses.AccountantGroups = accountantGroups;
                accountantGroupResponses.PageNumber = accountantGroupQueryPage.PageNumber;
                accountantGroupResponses.PageSize = accountantGroupQueryPage.PageSize;
                //計算總頁數
                accountantGroupResponses.TotalPage = TotalPageUtil.GetTotalPage(accountantGroupsQuery.Count(), accountantGroupQueryPage.PageSize);
                accountantGroupResponses.TotalCount = accountantGroupsQuery.Count();
                accountantGroupResponses.Success();
            }
            else
            {
                accountantGroupResponses.AccountantGroupNoData();
            }

            return accountantGroupResponses;
        }       

        /// <summary>
        /// 建立會計師群組資料
        /// </summary>
        /// <param name="accountantGroupForm">群組資料</param>        
        public ResponseViewModel CreateAccountantGroup(AccountantGroupForm accountantGroupForm)
        {
            ResponseViewModel response = new();

            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                .Where(accountantGroup => accountantGroup.AccountantGroupNumber == accountantGroupForm.AccountantGroupNumber)
                                                .FirstOrDefault();

            if (accountantGroupQuery == null)
            {
                AccountantGroup accountantGroup = mapper.Map<AccountantGroup>(accountantGroupForm);
                accountantGroup.CreateDate = DateTime.Now;
                dbContext.AccountantGroups.Add(accountantGroup);
                dbContext.SaveChanges();
                response.Success();                
            }
            else
            {
                response.CreateAccountantGroupNumberRepeat();
            }
            return response;
        }

    /// <summary>
    /// 更新會計師群組資料
    /// </summary>
    /// <param name="accountantGroupFormUpdate">群組資料</param>
    public ResponseViewModel UpdateAccountantGroup(AccountantGroupFormUpdate accountantGroupFormUpdate)
        {
            ResponseViewModel response = new();
            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                    .Where(accountantGroup => accountantGroup.Id == accountantGroupFormUpdate.Id)
                                                    .FirstOrDefault();
            if (accountantGroupQuery != null)
            {
                mapper.Map(accountantGroupFormUpdate, accountantGroupQuery);
                accountantGroupQuery.UpdateDate = DateTime.Now;
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.UpdateAccountantGroupNoData();
            }
            return response;
        }

        /// <summary>
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupId">群組Id</param>
        public ResponseViewModel DeleteAccountantGroup(int accountantGroupId)
        {
            ResponseViewModel response = new();
            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups.Find(accountantGroupId);

            if (accountantGroupQuery != null)
            {
                accountantGroupQuery.DeleteStatus = DeleteStatus.Yes;
                dbContext.Accountants.Where
                (
                        accountant =>
                        accountant.AccountantGroupId == accountantGroupId
                ).BatchUpdate(new Accountant { AccountantGroupId = 1 });                                
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DeleteAccountantGroupNoData();
            }
            return response;
        }
    }
}

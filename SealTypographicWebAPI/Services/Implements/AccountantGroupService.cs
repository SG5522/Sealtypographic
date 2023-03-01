using AutoMapper;
using EFCore.BulkExtensions;
using DBEntities.Consts;
using DBEntities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師群組管理
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

        ///<inheritdoc />
        public AccountantGroupList GetAll()
        {
            AccountantGroupList accountantGroupList = new();            

            List<AccountantGroup> accountantGroups = dbContext.AccountantGroups
                                                    .Where(accountantGroup => accountantGroup.DeleteStatus == DeleteStatus.No)
                                                    .ToList();
            if (accountantGroups.Any())
            {
                accountantGroupList.AccountantGroupDatas = mapper.Map<List<AccountantGroupViewModel>>(accountantGroups);                
                accountantGroupList.Success();                
            }
            else
            {
                accountantGroupList.AccountantGroupNoData();
            }

            return accountantGroupList;
        }

        ///<inheritdoc />
        public AccountantGroupResponse GetData(int accountantGroupId)
        {
            AccountantGroupResponse accountantGroupResponse = new();            

            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                    .FirstOrDefault(accountantGroup => accountantGroup.Id == accountantGroupId);                                                    

            if (accountantGroupQuery != null)
            {                
                accountantGroupResponse.AccountantGroupData = mapper.Map<AccountantGroupViewModel>(accountantGroupQuery);
                accountantGroupResponse.Success();                
            }
            else
            {
                accountantGroupResponse.AccountantGroupNoData();
            }

            return accountantGroupResponse;
        }

        ///<inheritdoc />
        public AccountantGroupPaginateViewModel GetPaginate(AccountantGroupSearch accountantGroupSearch)
        {
            AccountantGroupPaginateViewModel accountantGroupResponses = new();            
            
            IQueryable<AccountantGroup> accountantGroupsQuery = dbContext.AccountantGroups;
            if (!string.IsNullOrWhiteSpace(accountantGroupSearch.GroupName))
            {
                accountantGroupsQuery = accountantGroupsQuery.Where
                                        (
                                            accountantGroup =>                                            
                                            accountantGroup.Name.Contains(accountantGroupSearch.GroupName)
                                            && accountantGroup.AccountantGroupNumber.Contains(accountantGroupSearch.GroupName)
                                        );
            }
            accountantGroupsQuery.OrderBy(accountantGroup => accountantGroup.AccountantGroupNumber);

            if (accountantGroupsQuery.Any())
            {
                //取得該頁            
                List<AccountantGroup> thisPageAccountantGroups = accountantGroupsQuery
                                                                .Skip((accountantGroupSearch.PageNumber - 1) * accountantGroupSearch.PageSize)
                                                                .Take(accountantGroupSearch.PageSize)
                                                                .ToList();

                accountantGroupResponses.AccountantGroups = mapper.Map<List<AccountantGroupViewModel>>(thisPageAccountantGroups);                
                accountantGroupResponses.PageNumber = accountantGroupSearch.PageNumber;
                accountantGroupResponses.PageSize = accountantGroupSearch.PageSize;
                //計算總頁數
                accountantGroupResponses.TotalPage = TotalPageUtil.GetTotalPage(accountantGroupsQuery.Count(), accountantGroupSearch.PageSize);
                accountantGroupResponses.TotalCount = accountantGroupsQuery.Count();
                accountantGroupResponses.Success();
            }
            else
            {
                accountantGroupResponses.AccountantGroupNoData();
            }

            return accountantGroupResponses;
        }

        ///<inheritdoc />
        public ResponseViewModel New(AccountantGroupForm accountantGroupForm)
        {
            ResponseViewModel response = new();
            //確認編號是否重複
            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                .FirstOrDefault(accountantGroup => accountantGroup.AccountantGroupNumber == accountantGroupForm.AccountantGroupNumber);                                                

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

        ///<inheritdoc />
        public ResponseViewModel Update(AccountantGroupUpdateForm accountantGroupFormUpdate)
        {
            ResponseViewModel response = new();
            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups.Find(accountantGroupFormUpdate.Id);

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

        ///<inheritdoc />       
        public ResponseViewModel Delete(int accountantGroupId)
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

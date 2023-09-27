using AutoMapper;
using EFCore.BulkExtensions;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Utils;
using DBEntities;
using Microsoft.EntityFrameworkCore;
using DBEntities.Consts;
using SkiaSharp;
using AutoMapper.QueryableExtensions;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師群組管理
    /// </summary>
    public class AccountantGroupService : IAccountantGroupService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>        
        public AccountantGroupService(SealTypographicDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
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
            }
            accountantGroupList.Success();

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
            }
            accountantGroupResponse.Success();

            return accountantGroupResponse;
        }

        ///<inheritdoc />
        public AccountantGroupPaginateViewModel GetPaginate(AccountantGroupSearch accountantGroupSearch)
        {
            AccountantGroupPaginateViewModel accountantGroupResponses = new();
            int companyId = 1;

            IQueryable<AccountantGroup> accountantGroupsQuery = dbContext.AccountantGroups.Where
                                                                (
                                                                    x => x.DeleteStatus == DeleteStatus.No 
                                                                    && x.Company.Id == companyId
                                                                );
            if (!string.IsNullOrWhiteSpace(accountantGroupSearch.GroupName))
            {
                accountantGroupsQuery = accountantGroupsQuery.Where
                                        (
                                            accountantGroup =>                                            
                                            accountantGroup.Name.Contains(accountantGroupSearch.GroupName)
                                            || accountantGroup.Code.ToLower().Contains(accountantGroupSearch.GroupName.ToLower())
                                        );
            }
            accountantGroupsQuery.OrderBy(accountantGroup => accountantGroup.Code);

            if (accountantGroupsQuery.Any())
            {
                //取得該頁            
                //List<AccountantGroup> thisPageAccountantGroups = 

                accountantGroupResponses.AccountantGroups = accountantGroupsQuery
                                                            .Skip((accountantGroupSearch.PageNumber - 1) * accountantGroupSearch.PageSize)
                                                            .Take(accountantGroupSearch.PageSize)
                                                            .ProjectTo<AccountantGroupViewModel>(configurationProvider)
                                                            .ToList();

                accountantGroupResponses.PageNumber = accountantGroupSearch.PageNumber;
                accountantGroupResponses.PageSize = accountantGroupSearch.PageSize;
                //計算總頁數
                accountantGroupResponses.TotalPage = TotalPageUtil.GetTotalPage(accountantGroupsQuery.Count(), accountantGroupSearch.PageSize);
                accountantGroupResponses.TotalCount = accountantGroupsQuery.Count();      
            }
            accountantGroupResponses.Success();

            return accountantGroupResponses;
        }

        ///<inheritdoc />
        public ResponseViewModel New(AccountantGroupForm accountantGroupForm)
        {
            ResponseViewModel response = new();
            int companyId = 1;            

            //確認編號是否重複
            AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                .FirstOrDefault
                                                (
                                                    accountantGroup => 
                                                    accountantGroup.Code == accountantGroupForm.AccountantGroupNumber
                                                    && accountantGroup.DeleteStatus == DeleteStatus.No
                                                    && accountantGroup.Company.Id == companyId
                                                );                                                

            if (accountantGroupQuery == null)
            {                
                AccountantGroup accountantGroup = mapper.Map<AccountantGroup>(accountantGroupForm);
                accountantGroup.CreateDate = DateTime.Now;
                accountantGroup.Company = dbContext.Companys.Single(x => x.Id == companyId);
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
                accountantGroupQuery.Accountants = new List<Accountant>();
                dbContext.Remove(accountantGroupQuery);
                //dbContext.Accountants.Where
                //(
                //        accountant =>
                //        accountant.AccountantGroups.Remove == accountantGroupId
                //).BatchUpdate(new Accountant { AccountantGroupId = 1 });                                
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

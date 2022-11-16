using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.DbModels;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;


namespace SealTypographicWebAPI.Services.Accountant
{
    /// <summary>
    /// 勤業用 管理會計師群組
    /// </summary>
    public class AccountantGroupDeloitteService : IAccountantGroupService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ResponseService responseService;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="responseService"></param>        
        public AccountantGroupDeloitteService(SealTypographicDbContext dbContext, ResponseService responseService)
        {
            this.dbContext = dbContext;
            this.responseService = responseService;            
        }

        /// <summary>
        /// 取得會計師資料
        /// </summary>
        /// <param name="accountantGroupId">群組ID</param>
        /// <returns></returns>
        public AccountantGroupResponse GetAccountantGroup(string accountantGroupId)
        {
            AccountantGroupData accountantGroupData = new();
            Response response = new();
            var accountantGroupQuery = dbContext.AccountantGroups
                                        .Where(accountantGroup => accountantGroup.Id == accountantGroupId);

            if(accountantGroupQuery.Any())
            {
                var accountantGroup = accountantGroupQuery.First();

                accountantGroupData.Id = accountantGroup.Id;
                accountantGroupData.Name = accountantGroup.Name;

                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
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
        public AccountantGroupResponses GetAccountantGroups(AccountantGroupQueryPage accountantGroupQueryPage)
        {
            List<AccountantGroupData> accountantGroups = new();
            Response response = new();
            int totalPage = 0;
            int totalCount = 0;
            var accountantGroupsQuery = dbContext.AccountantGroups.AsQueryable();
            if(accountantGroupQueryPage.IdOrGroupsName != null)
            {
                accountantGroupsQuery = accountantGroupsQuery.Where
                                        (
                                            accountantGroup =>
                                            accountantGroup.Id.Contains(accountantGroupQueryPage.IdOrGroupsName)
                                            || accountantGroup.Name.Contains(accountantGroupQueryPage.IdOrGroupsName)
                                        );                                   
            }
            accountantGroupsQuery.OrderBy(accountantGroup => accountantGroup.Id);

            if (accountantGroupsQuery.Any()) 
            {
                //取得該頁            
                var thisPageAccountantGroups = accountantGroupsQuery.Skip((accountantGroupQueryPage.PageNumber - 1) * accountantGroupQueryPage.PageSize).Take(accountantGroupQueryPage.PageSize).ToList();
                //計算總頁數
                totalPage = (accountantGroupsQuery.Count() / accountantGroupQueryPage.PageSize) + (accountantGroupsQuery.Count() % accountantGroupQueryPage.PageSize == 0 ? 0 : 1);
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
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }

            return new AccountantGroupResponses()
            {
                PageNumber = accountantGroupQueryPage.PageNumber,
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
        public Response CreateAccountantGroup(AccountantGroupData accountantGroupData)
        {
            Response response = new();
            var accountantGroupQuery = dbContext.AccountantGroups
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
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.UniqueConstraintFailed);
            }
            return response;
        }

        /// <summary>
        /// 更新會計師群組資料
        /// </summary>
        /// <param name="accountantGroupData">群組資料</param>
        public Response UpdateAccountantGroup(AccountantGroupData accountantGroupData)
        {
            Response response = new();
            var accountantGroupQuery = dbContext.AccountantGroups
                                .Where(accountantGroup => accountantGroup.Id == accountantGroupData.Id);

            if (accountantGroupQuery.Any())
            {
                AccountantGroup accountantGroup = accountantGroupQuery.First();
                accountantGroup.Id = accountantGroupData.Id;
                accountantGroup.Name = accountantGroupData.Name;
                dbContext.SaveChanges();
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }
            return response;
        }

        /// <summary>
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupDataId"></param>
        public Response DeleteAccountantGroup(string accountantGroupDataId)
        {
            Response response = new();            
            var accountantGroupQuery = dbContext.AccountantGroups.Where
                                       (
                                            accountantGroup => 
                                            accountantGroup.Id == accountantGroupDataId
                                       );
            if (accountantGroupQuery.Any())
            {
                var accountantQuery = dbContext.Accountants.Where
                                   (
                                         accountant =>
                                         accountant.AccountantGroupId == accountantGroupDataId
                                   ).BatchUpdate(new DbModels.Accountant { AccountantGroupId = "0" });

                AccountantGroup accountantGroup = accountantGroupQuery.First();
                dbContext.AccountantGroups.Remove(accountantGroup);
                dbContext.SaveChanges();
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }
            return response;
        }
    }
}

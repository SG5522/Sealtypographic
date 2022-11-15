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
        /// 依搜尋條件獲得會計資料列表
        /// </summary>
        /// <param name="idOrGroupsName">會計師群組ID或是群組名稱</param>
        /// <param name="thisPage">現在頁次</param>
        /// <param name="pageSize">單頁資料量</param>     
        /// <returns></returns>
        public AccountantGroupResponses GetAccountantGroups(string idOrGroupsName, int thisPage, int pageSize)
        {
            List<AccountantGroupData> accountantGroups = new();
            Response response = new();
            int totalPage = 0;
            int totalCount = 0;
            var accountantGroupsQuery = dbContext.AccountantGroups.Where
                                        (
                                            accountantGroup =>
                                            accountantGroup.Id.Contains(idOrGroupsName)
                                            || accountantGroup.Name.Contains(idOrGroupsName)
                                        )
                                        .OrderBy(accountantGroup => accountantGroup.Id);
            if(accountantGroupsQuery.Any()) 
            {
                //取得該頁            
                var thisPageAccountantGroups = accountantGroupsQuery.Skip((thisPage - 1) * pageSize).Take(pageSize).ToList();
                //計算總頁數
                totalPage = (accountantGroupsQuery.Count() / pageSize) + (accountantGroupsQuery.Count() % pageSize == 0 ? 0 : 1);
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
                ThisPage = thisPage,
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

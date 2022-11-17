using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.DbModels;
using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Services.Accountant
{
    /// <summary>
    /// 勤業用 管理會計師資料
    /// </summary>
    public class AccountantDeloitteService : IAccountantService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ResponseService responseService;
        private readonly StatusService statusService;

        /// <summary>
        /// 注入DB、ResponseService、StatusService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="responseService"></param>
        /// <param name="statusService"></param>
        public AccountantDeloitteService(SealTypographicDbContext dbContext, ResponseService responseService, StatusService statusService)
        {
            this.dbContext = dbContext;
            this.responseService = responseService;
            this.statusService = statusService;
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
            var accountantQuery = from accountant in dbContext.Set<DbModels.Accountant>()
                                  join accountantGroup in dbContext.Set<AccountantGroup>()
                                  on accountant.AccountantGroupId equals accountantGroup.Id
                                  where accountant.Id == accountantId
                                  select new 
                                  {
                                      accountant.Id,
                                      accountant.Name,
                                      accountant.AvailableDate,
                                      accountant.CreateDate,
                                      accountant.AccountantGroupId,
                                      accountantGroupName = accountantGroup.Name
                                  };
         
            if (accountantQuery.Any())
            {
                var accountant = accountantQuery.First();

                accountantViewModel.Id = accountant.Id;
                accountantViewModel.Name = accountant.Name;
                accountantViewModel.AvailableDate = accountant.AvailableDate;
                accountantViewModel.CreateDate = accountant.CreateDate;
                accountantViewModel.AccountantGroupsId = accountant.AccountantGroupId;
                accountantViewModel.AccountantGroupsName = accountant.accountantGroupName;

                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
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
            int totalPage = 0;
            int totalCount = 0;
            var accountantsQuery = from accountant in dbContext.Set<DbModels.Accountant>()
                                    join accountantGroup in dbContext.Set<AccountantGroup>()
                                    on accountant.AccountantGroupId equals accountantGroup.Id
                                    select new
                                    {
                                        accountant.Id,
                                        accountant.Name,
                                        accountant.AvailableDate,
                                        accountant.AccountantGroupId,
                                        accountantGroupName = accountantGroup.Name,
                                        accountant.Status
                                    };

            if(accountantQueryPage.IdOrNameOrGroupsName != null)
            {
                accountantsQuery = accountantsQuery.Where
                                    (
                                        accountant =>                   
                                        accountant.Id.Contains(accountantQueryPage.IdOrNameOrGroupsName)
                                        || accountant.Name.Contains(accountantQueryPage.IdOrNameOrGroupsName)
                                        || accountant.accountantGroupName.Contains(accountantQueryPage.IdOrNameOrGroupsName)
                                    );
            }
            accountantsQuery = accountantsQuery.OrderBy( accountant => accountant.Id );
                                
            if (accountantsQuery.Any())
            {
                //取得該頁            
                var thisPageAccountants = accountantsQuery
                                          .Skip((accountantQueryPage.PageNumber - 1) * accountantQueryPage.PageSize)
                                          .Take(accountantQueryPage.PageSize)
                                          .ToList();
                //計算總頁數
                totalPage = (accountantsQuery.Count() / accountantQueryPage.PageSize) + (accountantsQuery.Count() % accountantQueryPage.PageSize == 0 ? 0 : 1);
                totalCount = accountantsQuery.Count();
                foreach (var accountant in thisPageAccountants)
                {
                    accountantViewModels.Add(new AccountantViewModel()
                    {
                        Id = accountant.Id,                        
                        Name = accountant.Name,
                        AvailableDate = accountant.AvailableDate,
                        AccountantGroupsId = accountant.AccountantGroupId,
                        AccountantGroupsName = accountant.accountantGroupName,
                        StatusString = statusService.Get((Status)accountant.Status)
                    });
                }
                //取得成功訊息
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
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
        /// <param name="accountantBaseData">基本資料</param>
        /// <returns></returns>
        public Response CreateAccountant(AccountantBaseData accountantBaseData)
        {
            Response response = new();
            var accountantQuery = dbContext.Accountants
                                    .Where(accountant => accountant.Id == accountantBaseData.Id);

            if(!accountantQuery.Any())
            {
                DbModels.Accountant accountant = new()
                {
                    Id = accountantBaseData.Id,
                    Name = accountantBaseData.Name,
                    AvailableDate = accountantBaseData.AvailableDate,
                    //CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                    CreateDate = accountantBaseData.CreateDate,
                    AccountantGroupId = accountantBaseData.AccountantGroupsId,
                    //Status = 0, //預設建立是待審
                    Status = accountantBaseData.Status
                    
                };
                dbContext.Accountants.Add(accountant);
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
        /// 更新會計師基本資料
        /// </summary>
        /// <param name="accountantBaseData">會計師基本資料 accountantBaseData.id 為搜尋條件</param>        
        public Response UpdateAccountant(AccountantBaseData accountantBaseData)
        {
            Response response = new();
            var accountantQuery = dbContext.Accountants
                                .Where(accountant => accountant.Id == accountantBaseData.Id);

            if (accountantQuery.Any())
            {
                DbModels.Accountant accountant = accountantQuery.First();
                accountant.Id = accountantBaseData.Id;
                accountant.Name = accountantBaseData.Name;
                accountant.AvailableDate = accountantBaseData.AvailableDate;               
                accountant.AccountantGroupId = accountantBaseData.AccountantGroupsId;
                accountant.Status = accountantBaseData.Status;
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
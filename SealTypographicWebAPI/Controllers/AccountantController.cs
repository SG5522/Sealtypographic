using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Services;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師基本資料
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]    
    public class AccountantController : APIControllerBase
    {
        /// <summary>
        /// 會計師資料管理Service
        /// </summary>
        private readonly IAccountantService accountantService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="accountantService">管理會計師資料</param>
        /// <param name="applicationUserService"></param>
        public AccountantController(IAccountantService accountantService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.accountantService = accountantService;
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        /// <returns></returns>
        [HttpGet("{accountantId}")]
        public async Task<AccountantDetailResponse> Detail(int accountantId) => await accountantService.GetDetail(accountantId, await GetUserId());

        /// <summary>
        /// 依搜尋條件獲得資料列表(分頁)
        /// </summary>
        /// <param name="accountantSearch">搜尋條件</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AccountantPaginateViewModel> Paginate([FromQuery]AccountantSearch accountantSearch) 
            => await accountantService.GetPaginate(accountantSearch, false, await GetUserId());

        /// <summary>
        /// 依搜尋條件獲得資料列表(排版使用)
        /// </summary>
        /// <param name="accountantSearch">搜尋條件</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<AccountantPaginateViewModel> PaginateWithTypographic([FromQuery] AccountantSearch accountantSearch) 
            => await accountantService.GetPaginate(accountantSearch, true, await GetUserId());

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="accountantForm">會計師資料</param>
        [HttpPost]
        public async Task<AccountantCreateResponse> New(AccountantForm accountantForm) => await accountantService.New(accountantForm, await GetUserId());

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="accountantFormUpdate">會計師資料(Id為查詢用)</param>        
        [HttpPut]
        public async Task<ResponseViewModel> Update(AccountantUpdateForm accountantFormUpdate) => await accountantService.Update(accountantFormUpdate, await GetUserId());

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        [HttpDelete("{accountantId}")]
        public async Task<ResponseViewModel> Delete(int accountantId) => await accountantService.Delete(accountantId, await GetUserId());
    }
}

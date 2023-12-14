using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Services;
using Serilog;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師基本資料
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    [Authorize]
    public class AccountantController : ControllerBase
    {
        /// <summary>
        /// 會計師資料管理Service
        /// </summary>
        private readonly IAccountantService accountantService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="accountantService">管理會計師資料</param>
        public AccountantController(IAccountantService accountantService)
        {
            this.accountantService = accountantService;
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        /// <returns></returns>
        [HttpGet("{accountantId}")]
        public AccountantDetailResponse Detail(int accountantId) => accountantService.GetDetail(accountantId);

        /// <summary>
        /// 依搜尋條件獲得資料列表(分頁)
        /// </summary>
        /// <param name="accountantSearch">搜尋條件</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantPaginateViewModel Paginate([FromQuery]AccountantSearch accountantSearch) 
            => accountantService.GetPaginate(accountantSearch, false);

        /// <summary>
        /// 依搜尋條件獲得資料列表(排版使用)
        /// </summary>
        /// <param name="accountantSearch">搜尋條件</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantPaginateViewModel PaginateWithTypographic([FromQuery] AccountantSearch accountantSearch) 
            => accountantService.GetPaginate(accountantSearch, true);

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="accountantForm">會計師資料</param>
        [HttpPost]
        public AccountantCreateResponse New(AccountantForm accountantForm) => accountantService.New(accountantForm);

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="accountantFormUpdate">會計師資料(Id為查詢用)</param>        
        [HttpPut]
        public ResponseViewModel Update(AccountantUpdateForm accountantFormUpdate) => accountantService.Update(accountantFormUpdate);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        [HttpDelete("{accountantId}")]
        public ResponseViewModel Delete(int accountantId) => accountantService.Delete(accountantId);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Services;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理客戶基本資料
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = KeycloakRoleConsts.DATAMANAGE_CUSTOMER)]
    public class CustomerController : APIControllerBase
    {
        /// <summary>
        /// 管理客戶資料的Service
        /// </summary>
        private readonly ICustomerService customerService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="customerService">管理客戶資料的Service</param>
        /// <param name="applicationUserService">GetUserInfoService</param>
        public CustomerController(ICustomerService customerService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.customerService = customerService;                        
        }

        /// <summary>
        /// 取得客戶資料列表(分頁)
        /// </summary>
        /// <param name="customerSearch">客戶分頁搜尋</param>        
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<CustomerPaginateSummary> Paginate([FromQuery] CustomerSearch customerSearch) => 
            await customerService.GetPaginate(customerSearch, await GetUserId());

        /// <summary>
        /// 取得客戶詳細基本資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        public async Task<CustomerDetailViewModel> Detail(int customerId) => await customerService.GetDetail(customerId, await GetUserInfo());

        /// <summary>
        /// 新增客戶基本資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CreateCustomerResponse> New(CustomerForm customerForm) => await customerService.New(customerForm, await GetUserId());

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="customerUpdateForm">基本資料</param>
        [HttpPut]
        public async Task<ResponseViewModel> Update(CustomerUpdateForm customerUpdateForm) =>await customerService.Update(customerUpdateForm, await GetUserId());

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="customerId">客戶Id</param>
        /// <returns></returns>
        [HttpDelete("{customerId}")]
        public async Task<ResponseViewModel> Delete(int customerId) => await customerService.Delete(customerId, await GetUserId());
    }
}

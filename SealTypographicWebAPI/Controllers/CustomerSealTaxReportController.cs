using DBEntities.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSeal;
using SealTypographicWebAPI.Services;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理客戶稅報印鑑
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = KeycloakRoleConsts.TAXREPORT_SEALDATA)]
    public class CustomerSealTaxReportController : APIControllerBase
    {
        /// <summary>
        /// 客戶印鑑管理的service
        /// </summary>
        private readonly ICustomerSealService customerSealService;

        /// <summary>
        /// 建構:注入客戶印鑑管理的service
        /// </summary>
        /// <param name="customerSealService">客戶印鑑管理的service</param>
        /// <param name="applicationUserService"></param>       
        public CustomerSealTaxReportController(ICustomerSealService customerSealService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.customerSealService = customerSealService;
        }

        /// <summary>
        /// 取得客戶列表
        /// (含有稅報資料才列出)
        /// </summary>
        /// <param name="customerSearch">客戶列表搜尋</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<CustomerPaginateViewModel> Paginate([FromQuery] CustomerSearch customerSearch) 
            => await customerSealService.GetPaginate(customerSearch, TypographyType.TaxReport, await GetUserId()); 

        /// <summary>
        /// 取得客戶印鑑稅報年度表
        /// </summary>
        /// <param name="customerSealQuarterPaginateSearch">印鑑年度分頁搜尋</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<CustomerSealQuarterPaginateViewModel> Year([FromQuery] CustomerSealQuarterPaginateSearch customerSealQuarterPaginateSearch) 
            => await customerSealService.GetQuarterYear(customerSealQuarterPaginateSearch, false, TypographyType.TaxReport, await GetUserId());

        /// <summary>
        /// 取得客戶印鑑稅報年度表(排版使用)
        /// </summary>
        /// <param name="customerSealQuarterPaginateSearch">印鑑季度分頁搜尋</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<CustomerSealQuarterPaginateViewModel> YearWithTypographic([FromQuery] CustomerSealQuarterPaginateSearch customerSealQuarterPaginateSearch) 
            => await customerSealService.GetQuarterYear(customerSealQuarterPaginateSearch, true, TypographyType.TaxReport, await GetUserId());

        /// <summary>
        /// 取得客戶印鑑群組簡短訊息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="quarterId"></param>        
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<CustomerSealGroupResponse> CustomerSealGroupSummary(int customerId, int quarterId) 
            => await customerSealService.GetCustomerSealGroupSummry(customerId, quarterId, await GetUserId());

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <param name="isTransparent" example="false">是否白底透明化處理</param>             
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public async Task<CustomerSealViewModels> Seals(int customerSealQuarterId, bool isTransparent) 
            => await customerSealService.GetSeals(customerSealQuarterId, isTransparent, await GetUserInfo());

        /// <summary>
        /// 新增客戶印鑑組資料
        /// </summary>        
        /// <param name="customerSealForms">客戶印鑑組資料</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(CustomerSealForm customerSealForms) 
            => await customerSealService.New(customerSealForms, TypographyType.TaxReport, await GetUserInfo());

        /// <summary>
        /// 異動客戶印鑑
        /// </summary>        
        /// <param name="customerSealUpdate">需要異動客戶印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<List<ResponseViewModel>> Update(CustomerSealUpdate customerSealUpdate) 
            => await customerSealService.Update(customerSealUpdate, await GetUserInfo());

        /// <summary>
        /// 此季度印鑑從草稿狀態變更為待審
        /// </summary>        
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Pending(int customerSealQuarterId) 
            => await customerSealService.ChangeReviewStatus(customerSealQuarterId, ReviewStatus.Pending, await GetUserInfo());

        /// <summary>
        /// 此季度印鑑從草稿狀態變更為作廢
        /// </summary>        
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Invalid(int customerSealQuarterId) 
            => await customerSealService.ChangeReviewStatus(customerSealQuarterId, ReviewStatus.Invalid, await GetUserInfo());

        /// <summary>
        /// 此季度印鑑從待審狀態變更為草稿(收回)
        /// </summary>        
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> CancelReview(int customerSealQuarterId) 
            => await customerSealService.ChangeReviewStatus(customerSealQuarterId, ReviewStatus.Draft, await GetUserInfo());
    }
}

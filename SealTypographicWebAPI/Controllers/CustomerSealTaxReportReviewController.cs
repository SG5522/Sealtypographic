using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Services;
using Serilog;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 客戶印鑑審核(稅報)
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    public class CustomerSealTaxReportReviewController : APIControllerBase
    {
        /// <summary>
        /// 客戶印鑑審核管理的service(稅報)
        /// </summary>
        private readonly ICustomerSealReviewService customerSealReviewService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="customerSealReviewService">客戶印鑑審核管理</param>
        /// <param name="applicationUserService"></param>
        public CustomerSealTaxReportReviewController(ICustomerSealReviewService customerSealReviewService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.customerSealReviewService = customerSealReviewService;
        }
       
        /// <summary>
        /// 客戶印鑑待審清單
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<CustomerSealGroupReviewPaginate> ReviewPaginate([FromQuery]CustomerSealSearchReview customerSealReviewSearch) => 
            await customerSealReviewService.GetReviewList(customerSealReviewSearch, TypographyType.TaxReport, await GetUserInfo());

        /// <summary>
        /// 客戶基本資料與該年所有印鑑
        /// </summary>
        /// <param name="customerSealQuarterId"></param>        
        /// <returns></returns>
        [HttpGet("{customerSealQuarterId}")]
        public async  Task<CustomerSealGroupDetailReviewResponse> ReviewDetail(int customerSealQuarterId) => 
            await customerSealReviewService.GetReviewDetail(customerSealQuarterId, await GetUserInfo());

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Approval(List<int> customerSealQuarterIds) => 
            await customerSealReviewService.StatusChange(customerSealQuarterIds, ReviewStatus.Approval, await GetUserInfo());

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Reject(List<int> customerSealQuarterIds) => 
            await customerSealReviewService.StatusChange(customerSealQuarterIds, ReviewStatus.Reject, await GetUserInfo());

        /// <summary>
        /// 審核不受理
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Refuse(List<int> customerSealQuarterIds) => 
            await customerSealReviewService.StatusChange(customerSealQuarterIds, ReviewStatus.Refuse, await GetUserInfo());
    }
}

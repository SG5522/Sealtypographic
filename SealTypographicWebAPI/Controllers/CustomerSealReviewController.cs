using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Services;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 客戶印鑑審核(財報)
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    public class CustomerSealReviewController : APIControllerBase
    {
        /// <summary>
        /// 客戶印鑑審核管理的service(財報)
        /// </summary>
        private readonly ICustomerSealReviewService customerSealReviewService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="customerSealReviewService">客戶印鑑審核管理</param>
        /// <param name="applicationUserService"></param>
        public CustomerSealReviewController(ICustomerSealReviewService customerSealReviewService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.customerSealReviewService = customerSealReviewService;
        }
       
        /// <summary>
        /// 客戶印鑑待審清單
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<CustomerSealGroupReviewPaginate> ReviewPaginate([FromQuery]CustomerSealSearchReview customerSealReviewSearch) => 
             await customerSealReviewService.GetReviewList(customerSealReviewSearch, TypographyType.FinancialReport, await GetUserId());

        /// <summary>
        /// 客戶基本資料與該季所有印鑑
        /// </summary>
        /// <param name="customerSealQuarterId"></param>        
        /// <returns></returns>
        [HttpGet("{customerSealQuarterId}")]
        public async Task<CustomerSealGroupDetailReviewResponse> ReviewDetail(int customerSealQuarterId) => 
            await customerSealReviewService.GetReviewDetail(customerSealQuarterId, await GetUserId());

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Approval(List<int> customerSealQuarterIds) => 
            await customerSealReviewService.StatusChange(customerSealQuarterIds, ReviewStatus.Approval, await GetUserId());

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Reject(List<int> customerSealQuarterIds) => 
            await customerSealReviewService.StatusChange(customerSealQuarterIds, ReviewStatus.Reject, await GetUserId());

        /// <summary>
        /// 審核不受理
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Refuse(List<int> customerSealQuarterIds) => 
            await customerSealReviewService.StatusChange(customerSealQuarterIds, ReviewStatus.Refuse, await GetUserId());
    }
}

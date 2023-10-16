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
    public class CustomerSealTaxReportReviewController : ControllerBase
    {
        /// <summary>
        /// 客戶印鑑審核管理的service(稅報)
        /// </summary>
        private readonly ICustomerSealReviewService customerSealReviewService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="customerSealReviewService">客戶印鑑審核管理</param>
        public CustomerSealTaxReportReviewController(ICustomerSealReviewService customerSealReviewService)
        {
            this.customerSealReviewService = customerSealReviewService;
        }
       
        /// <summary>
        /// 客戶印鑑待審清單
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealGroupReviewPaginate ReviewPaginate([FromQuery]CustomerSealSearchReview customerSealReviewSearch) => customerSealReviewService.GetReviewList(customerSealReviewSearch, TypographyType.TaxReport);

        /// <summary>
        /// 客戶基本資料與該年所有印鑑
        /// </summary>
        /// <param name="customerSealQuarterId"></param>        
        /// <returns></returns>
        [HttpGet("{customerSealQuarterId}")]
        public CustomerSealGroupDetailReviewResponse ReviewDetail(int customerSealQuarterId) => customerSealReviewService.GetReviewDetail(customerSealQuarterId);

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public ResponseViewModel Approval(List<int> customerSealQuarterIds) => customerSealReviewService.StatusChange(customerSealQuarterIds, ReviewStatus.Approval, 0);

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public ResponseViewModel Reject(List<int> customerSealQuarterIds) => customerSealReviewService.StatusChange(customerSealQuarterIds, ReviewStatus.Reject, 0);

        /// <summary>
        /// 審核不受理
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public ResponseViewModel Refuse(List<int> customerSealQuarterIds) => customerSealReviewService.StatusChange(customerSealQuarterIds, ReviewStatus.Refuse, 0);
    }
}

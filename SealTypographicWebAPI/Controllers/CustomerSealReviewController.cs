using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Services;
using Serilog;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 客戶印鑑審核
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    public class CustomerSealReviewController : ControllerBase
    {
        /// <summary>
        /// 客戶印鑑審核管理的service
        /// </summary>
        private readonly ICustomerSealReviewService customerSealReviewService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="customerSealReviewService">客戶印鑑審核管理</param>
        public CustomerSealReviewController(ICustomerSealReviewService customerSealReviewService)
        {
            this.customerSealReviewService = customerSealReviewService;
        }
       
        /// <summary>
        /// 客戶印鑑待審清單
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealQuarterReviewPaginate ReviewPaginate([FromQuery]CustomerSealSearchReview customerSealReviewSearch)
        {
            CustomerSealQuarterReviewPaginate customerSealReviewViewModelResponse = new();
            try
            {
                Log.Information("CustomerSealReview ReviewPaginate input {@Input}", customerSealReviewSearch);
                customerSealReviewViewModelResponse = customerSealReviewService.GetReviewList(customerSealReviewSearch);
                Log.Information("CustomerSealReview ReviewPaginate output {@Output}", customerSealReviewViewModelResponse);
                return customerSealReviewViewModelResponse;
            }
            catch (Exception ex) 
            {
                Log.Error("CustomerSealReview ReviewPaginate error {@Error}", ex);
                customerSealReviewViewModelResponse.DbError();
                return customerSealReviewViewModelResponse;
            }
        }

        /// <summary>
        /// 客戶基本資料與該季所有印鑑
        /// </summary>
        /// <param name="customerSealQuarterId"></param>        
        /// <returns></returns>
        [HttpGet("{customerSealQuarterId}")]
        public CustomerSealQuarterDetailReviewResponse ReviewDetail(int customerSealQuarterId)
        {                                    
            CustomerSealQuarterDetailReviewResponse customerSealReviewDetailResponse = new();
            try
            {
                Log.Information("CustomerSealQuarterReviewPaginate ReviewDetail input {@Input}", customerSealQuarterId);
                customerSealReviewDetailResponse = customerSealReviewService.GetReviewDetail(customerSealQuarterId);
                Log.Information("CustomerSealQuarterReviewPaginate ReviewDetail output {@Output}", customerSealReviewDetailResponse);
                return customerSealReviewDetailResponse;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealQuarterReviewPaginate ReviewDetail error {@Error}", ex);
                customerSealReviewDetailResponse.DbError();
                return customerSealReviewDetailResponse;
            }
        }

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="CustomerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public ResponseViewModel Approval(List<int> CustomerSealQuarterIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealReview Approval input {@Input}", CustomerSealQuarterIds);
                response = customerSealReviewService.Approval(CustomerSealQuarterIds);
                Log.Information("CustomerSealReview Approval output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealReview Approval error {@Error}", ex);
                response.DbError();
                return response;
            }
        }

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public ResponseViewModel Reject(List<int> customerSealQuarterIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealReview Reject input {@Input}", customerSealQuarterIds);
                response = customerSealReviewService.Reject(customerSealQuarterIds);
                Log.Information("CustomerSealReview Reject output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealReview Reject error {@Error}", ex);
                response.DbError();
                return response;
            }
        }

        /// <summary>
        /// 審核不受理
        /// </summary>
        /// <param name="customerSealQuarterIds">印鑑季度Id</param>
        [HttpPut("[Action]")]
        public ResponseViewModel Refuse(List<int> customerSealQuarterIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealReview Refuse input {@Input}", customerSealQuarterIds);
                response = customerSealReviewService.Refuse(customerSealQuarterIds);
                Log.Information("CustomerSealReview Refuse output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealReview Refuse error {@Error}", ex);
                response.DbError();
                return response;
            }
        }
    }
}

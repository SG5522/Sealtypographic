using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Services;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        [HttpGet("getCustomerSealReviewViewModel")]
        public CustomerSealQuarterViewModelResponse GetCustomerSealReviewViewModel([FromQuery]CustomerSealSearchReview customerSealReviewSearch)
        {
            CustomerSealQuarterViewModelResponse customerSealReviewViewModelResponse = new();
            try
            {
                Log.Information("CustomerSealQuarterViewModelResponse getCustomerSealReviewViewModel input {@Input}", customerSealReviewSearch);
                customerSealReviewViewModelResponse = customerSealReviewService.GetCustomerSealReviewViewModel(customerSealReviewSearch);
                Log.Information("CustomerSealQuarterViewModelResponse getCustomerSealReviewViewModel output {@Output}", customerSealReviewViewModelResponse);
                return customerSealReviewViewModelResponse;
            }
            catch (Exception ex) 
            {
                Log.Error("CustomerSealQuarterViewModelResponse getCustomerSealReviewViewModel error {@Error}", ex);
                customerSealReviewViewModelResponse.DbError();
                return customerSealReviewViewModelResponse;
            }
        }

        /// <summary>
        /// 客戶基本資料與該季所有印鑑
        /// </summary>
        /// <param name="customerSealReviewSearch"></param>
        /// <returns></returns>
        [HttpGet("getCustomerSealReviewDetail")]
        public CustomerSealReviewDetailResponse GetCustomerSealReviewDetail([FromQuery] CustomerSealSearchReview customerSealReviewSearch)
        {                                    
            CustomerSealReviewDetailResponse customerSealReviewDetailResponse = new();
            try
            {
                Log.Information("CustomerSealQuarterViewModelResponse getCustomerSealReviewDetail input {@Input}", customerSealReviewSearch);
                customerSealReviewDetailResponse = customerSealReviewService.GetCustomerSealReviewDetail(customerSealReviewSearch);
                Log.Information("CustomerSealQuarterViewModelResponse getCustomerSealReviewDetail output {@Output}", customerSealReviewSearch);
                return customerSealReviewDetailResponse;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealQuarterViewModelResponse getCustomerSealReviewDetail error {@Error}", ex);
                customerSealReviewDetailResponse.DbError();
                return customerSealReviewDetailResponse;
            }
        }

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="customerSealIds"></param>
        [HttpPut("Approval")]
        public ResponseViewModel PutApproval(List<int> customerSealIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealQuarterViewModelResponse PutApproval input {@Input}", customerSealIds);
                response = customerSealReviewService.ReviewApproval(customerSealIds);
                Log.Information("CustomerSealQuarterViewModelResponse PutApproval output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealQuarterViewModelResponse PutApproval error {@Error}", ex);
                response.DbError();
                return response;
            }
        }

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="customerSealIds"></param>
        [HttpPut("Reject")]
        public ResponseViewModel PutReject(List<int> customerSealIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealQuarterViewModelResponse PutReject input {@Input}", customerSealIds);
                response = customerSealReviewService.ReviewReject(customerSealIds);
                Log.Information("CustomerSealQuarterViewModelResponse PutReject output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealQuarterViewModelResponse PutReject error {@Error}", ex);
                response.DbError();
                return response;
            }
        }
    }
}

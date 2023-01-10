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
    [Produces("application/json")]
    [ApiController]
    public class CustomerSealReviewController : ControllerBase
    {
        /// <summary>
        /// 客戶印鑑審核管理的interface
        /// </summary>
        protected readonly ICustomerSealReviewService customerSealReviewService;

        /// <summary>
        /// 注入Service
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
        public CustomerSealReviewViewModelResponse GetCustomerSealReviewViewModel([FromQuery]CustomerSealReviewSearch customerSealReviewSearch)
        {
            CustomerSealReviewViewModelResponse customerSealReviewViewModelResponse = new();
            try
            {
                Log.Information("CustomerSealReviewViewModelResponse getCustomerSealReviewViewModel input {@Input}", customerSealReviewSearch);
                customerSealReviewViewModelResponse = customerSealReviewService.GetCustomerSealReviewViewModel(customerSealReviewSearch);
                Log.Information("CustomerSealReviewViewModelResponse getCustomerSealReviewViewModel output {@Output}", customerSealReviewViewModelResponse);
                return customerSealReviewViewModelResponse;
            }
            catch (Exception ex) 
            {
                Log.Error("CustomerSealReviewViewModelResponse getCustomerSealReviewViewModel error {@Error}", ex);
                customerSealReviewViewModelResponse.DbError();
                return customerSealReviewViewModelResponse;
            }
        }

        /// <summary>
        /// 客戶基本資料與該季所有印鑑
        /// </summary>
        /// <param name="customerSealQuarter"></param>
        /// <returns></returns>
        [HttpGet("getCustomerSealReviewDetail")]
        public CustomerSealReviewDetailResponse GetCustomerSealReviewDetail([FromQuery] CustomerSealQuarter customerSealQuarter)
        {                                    
            CustomerSealReviewDetailResponse customerSealReviewDetailResponse = new();
            try
            {
                Log.Information("CustomerSealReviewViewModelResponse getCustomerSealReviewDetail input {@Input}", customerSealQuarter);
                customerSealReviewDetailResponse = customerSealReviewService.GetCustomerSealReviewDetail(customerSealQuarter);
                Log.Information("CustomerSealReviewViewModelResponse getCustomerSealReviewDetail output {@Output}", customerSealQuarter);
                return customerSealReviewDetailResponse;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealReviewViewModelResponse getCustomerSealReviewDetail error {@Error}", ex);
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
                Log.Information("CustomerSealReviewViewModelResponse PutApproval input {@Input}", customerSealIds);
                response = customerSealReviewService.ReviewApproval(customerSealIds);
                Log.Information("CustomerSealReviewViewModelResponse PutApproval output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealReviewViewModelResponse PutApproval error {@Error}", ex);
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
                Log.Information("CustomerSealReviewViewModelResponse PutReject input {@Input}", customerSealIds);
                response = customerSealReviewService.ReviewReject(customerSealIds);
                Log.Information("CustomerSealReviewViewModelResponse PutReject output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealReviewViewModelResponse PutReject error {@Error}", ex);
                response.DbError();
                return response;
            }
        }
    }
}

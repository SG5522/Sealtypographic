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
        [HttpGet("[Action]")]
        public CustomerSealQuarterResponse ReviewQuarterSeals([FromQuery]CustomerSealSearchReview customerSealReviewSearch)
        {
            CustomerSealQuarterResponse customerSealReviewViewModelResponse = new();
            try
            {
                Log.Information("CustomerSealReview ReviewQuarterSeals input {@Input}", customerSealReviewSearch);
                customerSealReviewViewModelResponse = customerSealReviewService.GetReviewQuarterSeals(customerSealReviewSearch);
                Log.Information("CustomerSealReview ReviewQuarterSeals output {@Output}", customerSealReviewViewModelResponse);
                return customerSealReviewViewModelResponse;
            }
            catch (Exception ex) 
            {
                Log.Error("CustomerSealReview ReviewQuarterSeals error {@Error}", ex);
                customerSealReviewViewModelResponse.DbError();
                return customerSealReviewViewModelResponse;
            }
        }

        /// <summary>
        /// 客戶基本資料與該季所有印鑑
        /// </summary>
        /// <param name="CustomerSealQuarterId"></param>        
        /// <returns></returns>
        [HttpGet("getCustomerSealReviewDetail")]
        public CustomerSealReviewDetailResponse GetCustomerSealReviewDetail(int CustomerSealQuarterId)
        {                                    
            CustomerSealReviewDetailResponse customerSealReviewDetailResponse = new();
            try
            {
                Log.Information("CustomerSealQuarterResponse getCustomerSealReviewDetail input {@Input}", CustomerSealQuarterId);
                customerSealReviewDetailResponse = customerSealReviewService.GetCustomerSealReviewDetail(CustomerSealQuarterId);
                Log.Information("CustomerSealQuarterResponse getCustomerSealReviewDetail output {@Output}", CustomerSealQuarterId);
                return customerSealReviewDetailResponse;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealQuarterResponse getCustomerSealReviewDetail error {@Error}", ex);
                customerSealReviewDetailResponse.DbError();
                return customerSealReviewDetailResponse;
            }
        }

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="customerSealIds"></param>
        [HttpPut("[Action]")]
        public ResponseViewModel Approval(List<int> customerSealIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealQuarterResponse PutApproval input {@Input}", customerSealIds);
                response = customerSealReviewService.ReviewApproval(customerSealIds);
                Log.Information("CustomerSealQuarterResponse PutApproval output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealQuarterResponse PutApproval error {@Error}", ex);
                response.DbError();
                return response;
            }
        }

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="customerSealIds"></param>
        [HttpPut("[Action]")]
        public ResponseViewModel Reject(List<int> customerSealIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealQuarterResponse PutReject input {@Input}", customerSealIds);
                response = customerSealReviewService.ReviewReject(customerSealIds);
                Log.Information("CustomerSealQuarterResponse PutReject output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealQuarterResponse PutReject error {@Error}", ex);
                response.DbError();
                return response;
            }
        }
    }
}

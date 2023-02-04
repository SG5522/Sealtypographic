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
        public CustomerSealQuarterReviewPaginate ReviewPaginate([FromQuery]CustomerSealSearchReview customerSealReviewSearch)
        {
            CustomerSealQuarterReviewPaginate customerSealReviewViewModelResponse = new();
            try
            {
                Log.Information("CustomerSealReview ReviewPaginate input {@Input}", customerSealReviewSearch);
                customerSealReviewViewModelResponse = customerSealReviewService.GetReviewQuarterSeals(customerSealReviewSearch);
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
        /// <param name="CustomerSealQuarterId"></param>        
        /// <returns></returns>
        [HttpGet("{CustomerSealQuarterId}")]
        public CustomerSealDetailReviewResponse ReviewDetail(int CustomerSealQuarterId)
        {                                    
            CustomerSealDetailReviewResponse customerSealReviewDetailResponse = new();
            try
            {
                Log.Information("CustomerSealQuarterReviewPaginate getCustomerSealReviewDetail input {@Input}", CustomerSealQuarterId);
                customerSealReviewDetailResponse = customerSealReviewService.GetCustomerSealReviewDetail(CustomerSealQuarterId);
                Log.Information("CustomerSealQuarterReviewPaginate getCustomerSealReviewDetail output {@Output}", CustomerSealQuarterId);
                return customerSealReviewDetailResponse;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealQuarterReviewPaginate getCustomerSealReviewDetail error {@Error}", ex);
                customerSealReviewDetailResponse.DbError();
                return customerSealReviewDetailResponse;
            }
        }

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="CustomerSealQuarterId"></param>
        [HttpPut("[Action]")]
        public ResponseViewModel Approval(List<int> CustomerSealQuarterId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealReview Approval input {@Input}", CustomerSealQuarterId);
                response = customerSealReviewService.ReviewApproval(CustomerSealQuarterId);
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
        /// <param name="CustomerSealQuarterId"></param>
        [HttpPut("[Action]")]
        public ResponseViewModel Reject(List<int> CustomerSealQuarterId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealReview Reject input {@Input}", CustomerSealQuarterId);
                response = customerSealReviewService.ReviewReject(CustomerSealQuarterId);
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
        /// <param name="CustomerSealQuarterId"></param>
        [HttpPut("[Action]")]
        public ResponseViewModel Refuse(List<int> CustomerSealQuarterId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealReview Refuse input {@Input}", CustomerSealQuarterId);
                response = customerSealReviewService.ReviewReject(CustomerSealQuarterId);
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

using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Services;
using Serilog;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 會計師簽印審核
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    public class AccountantSignReviewController : ControllerBase
    {
        /// <summary>
        /// 會計師簽印審核管理的service
        /// </summary>
        private readonly IAccountantSignReviewService accountSignReviewService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="accountSignReviewService">客戶印鑑審核管理</param>
        public AccountantSignReviewController(IAccountantSignReviewService accountSignReviewService)
        {
            this.accountSignReviewService = accountSignReviewService;
        }
       
        /// <summary>
        /// 會計師簽印組清單
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantSignGroupReviewPaginate ReviewPaginate([FromQuery]AccountantSignSearchReview accountantSignSearchReview)
        {
            AccountantSignGroupReviewPaginate AccountantSignReviewPaginate = new();
            try
            {
                Log.Information("AccountantSignReview ReviewPaginate input {@Input}", accountantSignSearchReview);
                AccountantSignReviewPaginate = accountSignReviewService.GetReviewPaginate(accountantSignSearchReview);
                Log.Information("AccountantSignReview ReviewPaginate output {@Output}", AccountantSignReviewPaginate);
                return AccountantSignReviewPaginate;
            }
            catch (Exception ex) 
            {
                Log.Error("AccountantSignReview ReviewPaginate error {@Error}", ex.Message); 
                AccountantSignReviewPaginate.DbError();
                return AccountantSignReviewPaginate;
            }
        }

        /// <summary>
        /// 會計師基本資料與簽印組
        /// </summary>
        /// <param name="accountantSignGroupId"></param>        
        /// <returns></returns>
        [HttpGet("{accountantSignGroupId}")]
        public AccountantSignGroupDetailReviewResponse ReviewDetail(int accountantSignGroupId)
        {
            AccountantSignGroupDetailReviewResponse accountantSignReviewDetailResponse = new();
            try
            {
                Log.Information("AccountantSignReview ReviewDetail input {@Input}", accountantSignGroupId);
                accountantSignReviewDetailResponse = accountSignReviewService.GetReviewDetail(accountantSignGroupId);
                Log.Information("AccountantSignReview ReviewDetail output {@Output}", accountantSignReviewDetailResponse);
                return accountantSignReviewDetailResponse;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignReview ReviewDetail error {@Error}", ex.Message); 
                accountantSignReviewDetailResponse.DbError();
                return accountantSignReviewDetailResponse;
            }
        }

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        [HttpPut("[Action]")]
        public ResponseViewModel Approval(List<int> accountantSignGroupIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSignReview Approval input {@Input}", accountantSignGroupIds);
                response = accountSignReviewService.Approval(accountantSignGroupIds);
                Log.Information("AccountantSignReview Approval output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignReview Approval error {@Error}", ex.Message); 
                response.DbError();
                return response;
            }
        }

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        [HttpPut("[Action]")]
        public ResponseViewModel Reject(List<int> accountantSignGroupIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSignReview Reject input {@Input}", accountantSignGroupIds);
                response = accountSignReviewService.Reject(accountantSignGroupIds);
                Log.Information("AccountantSignReview Reject output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignReview Reject error {@Error}", ex.Message); 
                response.DbError();
                return response;
            }
        }

        /// <summary>
        /// 審核不受理
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        [HttpPut("[Action]")]
        public ResponseViewModel Refuse(List<int> accountantSignGroupIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSignReview Refuse input {@Input}", accountantSignGroupIds);
                response = accountSignReviewService.Refuse(accountantSignGroupIds);
                Log.Information("AccountantSignReview Refuse output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignReview Refuse error {@Error}", ex.Message); 
                response.DbError();
                return response;
            }
        }
    }
}

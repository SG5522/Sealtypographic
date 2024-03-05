using DBEntities.Consts;
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
        public async Task<AccountantSignGroupReviewPaginate> ReviewPaginate([FromQuery]AccountantSignSearchReview accountantSignSearchReview) 
           => await accountSignReviewService.GetReviewPaginate(accountantSignSearchReview);

        /// <summary>
        /// 會計師基本資料與簽印組
        /// </summary>
        /// <param name="accountantSignGroupId"></param>        
        /// <returns></returns>
        [HttpGet("{accountantSignGroupId}")]
        public async Task<AccountantSignGroupDetailReviewResponse> ReviewDetail(int accountantSignGroupId)
             => await accountSignReviewService.GetReviewDetail(accountantSignGroupId);

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Approval(List<int> accountantSignGroupIds) 
            => await accountSignReviewService.StatusChange(accountantSignGroupIds, ReviewStatus.Approval);

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Reject(List<int> accountantSignGroupIds) 
            => await accountSignReviewService.StatusChange(accountantSignGroupIds, ReviewStatus.Reject);

        /// <summary>
        /// 審核不受理
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Refuse(List<int> accountantSignGroupIds) 
            => await accountSignReviewService.StatusChange(accountantSignGroupIds, ReviewStatus.Refuse);
    }
}

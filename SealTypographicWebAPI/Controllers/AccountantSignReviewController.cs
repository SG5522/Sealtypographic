using DBEntities.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Services;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 會計師簽印審核
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    [Authorize(Roles = KeycloakRoleConsts.REVIEW_ACCOUNTANTSIGNREVIEW)]
    public class AccountantSignReviewController : APIControllerBase
    {
        /// <summary>
        /// 會計師簽印審核管理的service
        /// </summary>
        private readonly IAccountantSignReviewService accountSignReviewService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="accountSignReviewService">客戶印鑑審核管理</param>
        /// <param name="applicationUserService"></param>
        public AccountantSignReviewController(IAccountantSignReviewService accountSignReviewService, 
            IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.accountSignReviewService = accountSignReviewService;
        }
       
        /// <summary>
        /// 會計師簽印組清單
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<AccountantSignReviewPaginate> ReviewPaginate([FromQuery]AccountantSignSearchReview accountantSignSearchReview) 
           => await accountSignReviewService.GetReviewPaginate(accountantSignSearchReview, await GetUserInfo());

        /// <summary>
        /// 會計師基本資料與簽印組
        /// </summary>
        /// <param name="accountantSignGroupId"></param>        
        /// <returns></returns>
        [HttpGet("{accountantSignGroupId}")]
        public async Task<AccountantSignDetailReviewResponse> ReviewDetail(int accountantSignGroupId)
             => await accountSignReviewService.GetReviewDetail(accountantSignGroupId, await GetUserInfo());

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Approval(List<int> accountantSignGroupIds) 
            => await accountSignReviewService.StatusChange(accountantSignGroupIds, ReviewStatus.Approval, await GetUserInfo());

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Reject(List<int> accountantSignGroupIds) 
            => await accountSignReviewService.StatusChange(accountantSignGroupIds, ReviewStatus.Reject, await GetUserInfo());

        /// <summary>
        /// 審核不受理
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Refuse(List<int> accountantSignGroupIds) 
            => await accountSignReviewService.StatusChange(accountantSignGroupIds, ReviewStatus.Refuse, await GetUserInfo());
    }
}

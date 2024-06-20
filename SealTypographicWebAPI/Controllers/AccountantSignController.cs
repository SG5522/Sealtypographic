using DBEntities.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.CustomerSeal;
using SealTypographicWebAPI.Services;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師簽印
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    [Authorize(Roles = KeycloakRoleConsts.DATAMANGE_ACCOUNTANT)]
    public class AccountantSignController : APIControllerBase
    {
        /// <summary>
        /// 會計師簽印管理Service
        /// </summary>
        private readonly IAccountantSignService accountantSignService;

        /// <summary>
        /// 建構：注入會計師簽印管理Service
        /// </summary>
        /// <param name="accountantSignService">會計師簽印管理Service</param>
        /// <param name="applicationUserService"></param>        
        public AccountantSignController(IAccountantSignService accountantSignService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.accountantSignService = accountantSignService;            
        }

        /// <summary>
        /// 取得會計師簽印建立日期列表
        /// </summary>
        /// <param name="accountantSignPaginateSearch">會計師簽印分頁搜尋</param>        
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<AccountantSignGroupResponse> GetCreateDates([FromQuery] AccountantSignPaginateSearch accountantSignPaginateSearch) 
                => await accountantSignService.GetCreateDates(accountantSignPaginateSearch, await GetUserId());

        /// <summary>
        /// 取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>
        /// <param name="isTransparent" example="false" >是否白底透明化</param>        
        /// <returns></returns>        
        [HttpGet]
        public async Task<AccountantSignViewModels> Signs(int accountantSignGroupId, bool isTransparent) 
                => await accountantSignService.GetSignViewModels(accountantSignGroupId, isTransparent, await GetUserInfo());

        /// <summary>
        /// 新增會計師簽印組
        /// </summary>
        /// <param name="accountantSignForms">會計師簽印組</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(AccountantSignForms accountantSignForms) => await accountantSignService.New(accountantSignForms, await GetUserInfo());

        /// <summary>
        /// 異動會計師簽印
        /// </summary>
        /// <param name="accountantSignUpdate">需要異動會計師簽印資料</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<List<ResponseViewModel>> Update(AccountantSignUpdate accountantSignUpdate) => await accountantSignService.Update(accountantSignUpdate, await GetUserInfo());

        /// <summary>
        /// 將草稿的簽印組狀態變更為待審
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>        
        /// <returns></returns>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Pending(int accountantSignGroupId) => await accountantSignService.ChangeReviewStatus(accountantSignGroupId, ReviewStatus.Pending);

        /// <summary>
        /// 將草稿的簽印組狀態變更為作廢
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> Invalid(int accountantSignGroupId) => await accountantSignService.ChangeReviewStatus(accountantSignGroupId, ReviewStatus.Invalid);

        /// <summary>
        /// 將待審的簽印組狀態變更為草稿
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> CancelReview(int accountantSignGroupId) => await accountantSignService.ChangeReviewStatus(accountantSignGroupId, ReviewStatus.Draft);
    }
}

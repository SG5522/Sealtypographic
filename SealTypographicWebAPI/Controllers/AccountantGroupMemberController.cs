using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models.AccountantGroupMember;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師群組成員
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]    
    public class AccountantGroupMemberController : APIControllerBase
    {
        /// <summary>
        /// 管理會計師群組成員的Service
        /// </summary>
        private readonly IAccountantGroupMemberService accountantGroupMemberService;

        /// <summary>
        /// 建構：注入Service
        /// </summary>
        /// <param name="accountantGroupMemberService">管理會計師群組成員的Service</param>
        /// <param name="applicationUserService"></param>   
        public AccountantGroupMemberController(IAccountantGroupMemberService accountantGroupMemberService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.accountantGroupMemberService = accountantGroupMemberService;
        }

        /// <summary>
        /// 取得群組成員資料
        /// </summary>
        /// <param name="accountantGroupMemberSearch">群組成員搜尋條件</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AccountantGroupMembers> Members([FromQuery] AccountantGroupMemberSearch accountantGroupMemberSearch) 
            => await accountantGroupMemberService.GetMembers(accountantGroupMemberSearch, true, await GetUserId());    

        /// <summary>
        /// 取得非該群組會計師列表
        /// </summary>
        /// <param name="accountantGroupMemberSearch">群組成員搜尋條件</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<AccountantGroupMembers> NotTheGroup([FromQuery] AccountantGroupMemberSearch accountantGroupMemberSearch) 
            => await accountantGroupMemberService.GetMembers(accountantGroupMemberSearch, false, await GetUserId());


        /// <summary>
        /// 更新會計師群組的成員
        /// </summary>
        /// <param name="accountantGroupMemberForm">會計師群組成員資料</param>       
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> UpdateGroupMembers(AccountantGroupMemberForm accountantGroupMemberForm) 
            => await accountantGroupMemberService.UpdateGroupMembers(accountantGroupMemberForm, await GetUserId());

    }
}

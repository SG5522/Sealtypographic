using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Util;
using Serilog;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.AccountantGroupMember;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師群組人員
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]    
    public class AccountantGroupMemberController : ControllerBase
    {
        /// <summary>
        /// 會計師群組成員管理的interface
        /// </summary>
        protected readonly IAccountantGroupMemberService accountantGroupMemberService;

        /// <summary>
        /// 會計師資料管理的interface
        /// </summary>
        protected readonly IAccountantService accountantService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="accountantGroupMemberService">會計師群組成員管理的interface</param>   
        /// <param name="accountantService">管理會計師interface</param>
        public AccountantGroupMemberController(IAccountantGroupMemberService accountantGroupMemberService, IAccountantService accountantService)
        {
            this.accountantGroupMemberService = accountantGroupMemberService;
            this.accountantService = accountantService;
        }

        /// <summary>
        /// 取得會計師群組成員資料
        /// </summary>
        /// <param name="accountantGroupMemberSearch">群組成員搜尋條件</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantGroupMembers GetAccountantGroupMember([FromQuery] AccountantGroupMemberSearch accountantGroupMemberSearch)
        {
            AccountantGroupMembers accountantGroupMembers = new ();
            try
            {
                Log.Information("AccountantGroupMember get FromQuery input {@Input}", accountantGroupMemberSearch);
                accountantGroupMembers = accountantGroupMemberService.GetAccountantGroupMembers(accountantGroupMemberSearch);                
                Log.Information("AccountantGroupMember get FromQuery output {@Output}", accountantGroupMembers);
                return accountantGroupMembers;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember get FromQuery error {@Error}", ex);
                accountantGroupMembers.DbError();                
                return accountantGroupMembers;
            }
        }

        /// <summary>
        /// 群組新增人員時取得非該群組成員資料
        /// </summary>
        /// <param name="notThisGroupMemberSearch">群組成員搜尋條件</param>
        /// <returns></returns>
        [HttpGet("NotTheGroup")]
        public NotThisGroupMember GetNotGroupMembers([FromQuery]NotThisGroupMemberSearch notThisGroupMemberSearch)
        {
            NotThisGroupMember notThisGroupMember = new ();
            try
            {                
                Log.Information("AccountantGroupMember get(NotTheGroup) input {@Input}", notThisGroupMemberSearch);
                notThisGroupMember = accountantGroupMemberService.GetNotThisGroupMember(notThisGroupMemberSearch);
                Log.Information("AccountantGroupMember get(NotTheGroup) output {@Output}", notThisGroupMember);
                return notThisGroupMember;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember get(NotTheGroup) error {@Error}", ex);                
                notThisGroupMember.DbError();
                return notThisGroupMember;
            }
        }

        /// <summary>
        /// 取得會計師群組成員資料(單筆)
        /// </summary>
        /// <param name="accountantId">會計師ID</param>
        /// <returns></returns>
        [HttpGet("{accountantId}")]
        public AccountantDetailResponse GetAccountantViewModel(int accountantId)
        {
            AccountantDetailResponse accountantResponse = new();
            try
            {
                Log.Information("AccountantGroupMember get{accountantId} input {@Input}", accountantId);
                accountantResponse = accountantService.GetAccountant(accountantId);
                Log.Information("AccountantGroupMember get{accountantId} output {@Output}", accountantResponse);
                return accountantResponse;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember get{accountantId} error {@Error}", ex);                
                accountantResponse.DbError();
                return accountantResponse;
            }
        }

        /// <summary>
        /// 變更會計師群組(單個)
        /// </summary>
        /// <param name="accountantGroupChangeForm">會計群組變更資料</param>       
        [HttpPut]
        public ResponseViewModel PutAccountantGroup(AccountantGroupChangeForm accountantGroupChangeForm)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantGroupMember put input {@Input}", accountantGroupChangeForm);
                response = accountantGroupMemberService.UpdateAccountantGroup(accountantGroupChangeForm);
                Log.Information("AccountantGroupMember put output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember put error {@Error}", ex);
                response.DbError();
                return response;
            }
        }


        /// <summary>
        /// 新增人員時變更會計師的群組
        /// </summary>
        /// <param name="accountantGroupMemberForm">會計群組變更資料</param>       
        [HttpPut("NotTheGroup")]
        public ResponseViewModel PutGNotGroupMember(AccountantGroupMemberForm accountantGroupMemberForm)
        {
            ResponseViewModel response = new ();
            try
            {
                Log.Information("AccountantGroupMember put(NotTheGroup) input {@Input}", accountantGroupMemberForm);
                response = accountantGroupMemberService.ChangeNotTheGroupMember(accountantGroupMemberForm);
                Log.Information("AccountantGroupMember put(NotTheGroup) output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember put(NotTheGroup) error {@Error}", ex);
                response.DbError();
                return response;
            }
        }
    }
}

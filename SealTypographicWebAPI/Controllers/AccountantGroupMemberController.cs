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
            try
            {
                Log.Information("AccountantGroupMember get accountantGroupMembers input {@Input}", accountantGroupMemberSearch);
                AccountantGroupMembers accountantGroupMembers = accountantGroupMemberService.GetAccountantGroupMembers(accountantGroupMemberSearch);                
                Log.Information("AccountantGroupMember get accountantGroupMembers output {@Output}", accountantGroupMembers);
                return accountantGroupMembers;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember get accountantGroupMembers error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
                return new()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
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
            try
            {                
                Log.Information("AccountantGroupMember get  input {@Input}", notThisGroupMemberSearch);
                NotThisGroupMember notThisGroupMember =  accountantGroupMemberService.GetNotThisGroupMember(notThisGroupMemberSearch);
                Log.Information("AccountantGroupMember get  output {@Output}", notThisGroupMember);
                return notThisGroupMember;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember get  error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
                return new()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 取得會計師群組成員資料(單筆)
        /// </summary>
        /// <param name="accountantId">會計師ID</param>
        /// <returns></returns>
        [HttpGet("{accountantId}")]
        public AccountantResponse GetAccountantViewModel(int accountantId)
        {
            try
            {
                Log.Information("AccountantGroupMember get accountantViewModel input {@Input}", accountantId);
                AccountantResponse accountantResponse = accountantService.GetAccountant(accountantId);
                Log.Information("AccountantGroupMember get accountantViewModel output {@Output}", accountantResponse);
                return accountantResponse;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember get accountantGroupMembers error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
                return new()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 變更會計師群組(單個)
        /// </summary>
        /// <param name="accountantGroupChangeForm">會計群組變更資料</param>       
        [HttpPut]
        public ResponseViewModel PutAccountantGroup(AccountantGroupChangeForm accountantGroupChangeForm)
        {
            try
            {
                Log.Information("AccountantGroupMember put accountantGroupChangeForm input {@Input}", accountantGroupChangeForm);
                ResponseViewModel response = accountantGroupMemberService.UpdateAccountantGroup(accountantGroupChangeForm);
                Log.Information("AccountantGroupMember put accountantGroupChangeForm output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember put accountantGroupChangeForm error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }


        /// <summary>
        /// 新增人員時變更會計師的群組
        /// </summary>
        /// <param name="accountantGroupMemberForm">會計群組變更資料</param>       
        [HttpPut("NotTheGroup")]
        public ResponseViewModel PutGNotGroupMember(AccountantGroupMemberForm accountantGroupMemberForm)
        {
            try
            {
                Log.Information("AccountantGroupMember put accountantGroupChangeForm input {@Input}", accountantGroupMemberForm);
                ResponseViewModel response = accountantGroupMemberService.ChangeNotTheGroupMember(accountantGroupMemberForm);
                //Log.Information("AccountantGroupMember put accountantGroupChangeForm output {@Output}", response);
                //return ResponseUtil.Success();
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember put accountantGroupChangeForm error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }
    }
}

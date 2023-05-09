using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using Serilog;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.AccountantGroupMember;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師群組成員
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]    
    public class AccountantGroupMemberController : ControllerBase
    {
        /// <summary>
        /// 管理會計師群組成員的Service
        /// </summary>
        private readonly IAccountantGroupMemberService accountantGroupMemberService;

        /// <summary>
        /// 管理會計師資料的Service
        /// </summary>
        private readonly IAccountantService accountantService;

        /// <summary>
        /// 建構：注入Service
        /// </summary>
        /// <param name="accountantGroupMemberService">管理會計師群組成員的Service</param>   
        /// <param name="accountantService">管理會計師資料的Service</param>
        public AccountantGroupMemberController(IAccountantGroupMemberService accountantGroupMemberService, IAccountantService accountantService)
        {
            this.accountantGroupMemberService = accountantGroupMemberService;
            this.accountantService = accountantService;
        }

        /// <summary>
        /// 取得群組成員資料
        /// </summary>
        /// <param name="accountantGroupMemberSearch">群組成員搜尋條件</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantGroupMembers Members([FromQuery] AccountantGroupMemberSearch accountantGroupMemberSearch)
        {
            AccountantGroupMembers accountantGroupMembers = new ();
            try
            {
                Log.Information("AccountantGroupMember members input {@Input}", accountantGroupMemberSearch);
                accountantGroupMembers = accountantGroupMemberService.GetMembers(accountantGroupMemberSearch);                
                Log.Information("AccountantGroupMember members output {@Output}", accountantGroupMembers);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember FromQuery error {@Error}", ex.Message); 
                accountantGroupMembers.DbError();
            }
            return accountantGroupMembers;
        }

        /// <summary>
        /// 取得非該群組會計師列表
        /// </summary>
        /// <param name="notThisGroupMemberSearch">群組成員搜尋條件</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public NotThisGroupMember NotTheGroup([FromQuery]NotThisGroupMemberSearch notThisGroupMemberSearch)
        {
            NotThisGroupMember notThisGroupMember = new ();
            try
            {                
                Log.Information("AccountantGroupMember get NotTheGroup input {@Input}", notThisGroupMemberSearch);
                notThisGroupMember = accountantGroupMemberService.GetNotThisGroupMember(notThisGroupMemberSearch);
                Log.Information("AccountantGroupMember get NotTheGroup output {@Output}", notThisGroupMember);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember get NotTheGroup error {@Error}", ex.Message);                 
                notThisGroupMember.DbError();                
            }
            return notThisGroupMember;
        }

        /// <summary>
        /// 取得單筆會計師資料
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
                accountantResponse = accountantService.GetDetail(accountantId);
                Log.Information("AccountantGroupMember get{accountantId} output {@Output}", accountantResponse);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember get{accountantId} error {@Error}", ex.Message);                 
                accountantResponse.DbError();                
            }
            return accountantResponse;
        }

        /// <summary>
        /// 變更單個會計師的群組
        /// </summary>
        /// <param name="accountantGroupChangeForm">會計師群組資料</param>  
        /// <returns></returns>
        [HttpPut]
        public ResponseViewModel UpdateGroup(AccountantGroupChangeForm accountantGroupChangeForm)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantGroupMember put input {@Input}", accountantGroupChangeForm);
                response = accountantGroupMemberService.UpdateGroup(accountantGroupChangeForm);
                Log.Information("AccountantGroupMember put output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember put error {@Error}", ex.Message); 
                response.DbError();                
            }
            return response;
        }


        /// <summary>
        /// 變更多個會計師的群組
        /// </summary>
        /// <param name="accountantGroupMemberForm">會計師群組成員資料</param>       
        [HttpPut("NotTheGroup")]
        public ResponseViewModel UpdateNotGroupMembers(AccountantGroupMemberForm accountantGroupMemberForm)
        {
            ResponseViewModel response = new ();
            try
            {
                Log.Information("AccountantGroupMember UpdateNotGroupMembers input {@Input}", accountantGroupMemberForm);
                response = accountantGroupMemberService.ChangeNotTheGroupMember(accountantGroupMemberForm);
                Log.Information("AccountantGroupMember UpdateNotGroupMembers output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroupMember UpdateNotGroupMembers error {@Error}", ex.Message); 
                response.DbError();                
            }
            return response;
        }
    }
}

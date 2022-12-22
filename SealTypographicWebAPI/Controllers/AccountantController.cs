using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Services;
using Serilog;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師基本資料
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AccountantController : ControllerBase
    {
        /// <summary>
        /// 宣告會計師資料處理的interface
        /// </summary>
        protected readonly IAccountantService accountantService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="accountantService">管理會計師資料</param>
        public AccountantController(IAccountantService accountantService)
        {
            this.accountantService = accountantService;
        }

        /// <summary>
        /// 依搜尋條件獲得會計師資料列表
        /// </summary>
        /// <param name="accountantQueryPage">會計師分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantPaginatesViewModel Get([FromQuery]AccountantSearch accountantQueryPage)
        {
            AccountantPaginatesViewModel accountantPaginatesViewModel = new();
            try
            {
                Log.Information("Accountant get FromQuery input {@Input}", accountantQueryPage);
                accountantPaginatesViewModel = accountantService.GetAccountantViewModels(accountantQueryPage);
                Log.Information("Accountant get FromQuery output {@Output}", accountantPaginatesViewModel);
                return accountantPaginatesViewModel;
            }
            catch (Exception ex) 
            {
                Log.Error("AccountantGroups get FromQuery error {@Error}", ex);
                accountantPaginatesViewModel.DbError();                
                return accountantPaginatesViewModel;
            }
        }

        /// <summary>
        /// 取得會計師基本資料
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        /// <returns></returns>
        [HttpGet("{accountantId}")]
        public AccountantDetailResponse Get(int accountantId)
        {
            AccountantDetailResponse accountantResponse = new();
            try
            {
                Log.Information("Accountant get{accountantId} input {@Input}", accountantId);
                accountantResponse = accountantService.GetAccountant(accountantId);
                Log.Information("Accountant get{accountantId} output {@Output}", accountantResponse);
                return accountantResponse;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get{accountantId} error {@Error}", ex);
                accountantResponse.DbError();                
                return accountantResponse;
            }
        }

        /// <summary>
        /// 建立會計師基本資料
        /// </summary>
        /// <param name="accountantBaseData"></param>
        [HttpPost]
        public AccountantCreateResponse Post(AccountantForm accountantBaseData)
        {
            AccountantCreateResponse accountantCreateResponse = new();
            try
            {
                Log.Information("Accountant post input {@Input}", accountantBaseData);
                accountantCreateResponse = accountantService.CreateAccountant(accountantBaseData);
                Log.Information("Accountant post output {@Output}", accountantCreateResponse);
                return accountantCreateResponse;                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups post accountantFormUpdate error {@Error}", ex);
                accountantCreateResponse.DbError();
                return accountantCreateResponse;
            }
        }

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="accountantFormUpdate"></param>        
        [HttpPut]
        public ResponseViewModel Put(AccountantFormUpdate accountantFormUpdate)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("Accountant put input {@Input}", accountantFormUpdate);
                response = accountantService.UpdateAccountant(accountantFormUpdate);
                Log.Information("Accountant put output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups put accountantFormUpdate error {@Error}", ex);
                response.DbError();
                return response;
            }
        }

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="accountantId"></param>        
        [HttpDelete("{accountantId}")]
        public ResponseViewModel Delete(int accountantId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("Accountant delete(hide) input {@Input}", accountantId);
                response = accountantService.DeleteAccountant(accountantId);
                Log.Information("Accountant delete(hide) output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            { 
                Log.Error("AccountantGroups delete(hide) error {@Error}", ex);
                response.DbError();
                return response; 
            }
        }
    }
}

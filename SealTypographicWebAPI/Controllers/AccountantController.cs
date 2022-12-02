using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Util;
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
            try
            {
                Log.Information("Accountant get accountantViewModels input {@Input}", accountantQueryPage);
                AccountantPaginatesViewModel accountantResponses = accountantService.GetAccountantViewModels(accountantQueryPage);
                Log.Information("Accountant get accountantViewModels output {@Output}", accountantResponses);
                return accountantResponses;
            }
            catch (Exception ex) 
            {
                Log.Error("AccountantGroups get accountantViewModels error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.InternalServerError();
                return new ()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 取得會計師基本資料
        /// </summary>
        /// <param name="id" example="ACC001">會計師ID</param>        
        /// <returns></returns>
        [HttpGet("{id}")]
        public AccountantResponse Get(string id)
        {
            try
            {
                Log.Information("Accountant get accountantViewModel input {@Input}", id);
                AccountantResponse accountantResponse = accountantService.GetAccountant(id);
                Log.Information("Accountant get accountantViewModel output {@Output}", accountantResponse);
                return accountantResponse;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get accountantViewModel error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.InternalServerError();
                return new AccountantResponse()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 建立會計師基本資料
        /// </summary>
        /// <param name="accountantBaseData"></param>
        [HttpPost]
        public ResponseViewModel Post(AccountantForm accountantBaseData)
        {
            try
            {
                Log.Information("Accountant post accountantBaseData input {@Input}", accountantBaseData);
                ResponseViewModel response = accountantService.CreateAccountant(accountantBaseData);
                Log.Information("Accountant post accountantBaseData output {@Output}", response);
                return response;                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups post accountantBaseData error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="accountantBaseData"></param>        
        [HttpPut]
        public ResponseViewModel Put(AccountantForm accountantBaseData)
        {
            try
            {
                Log.Information("Accountant put accountantBaseData input {@Input}", accountantBaseData);
                ResponseViewModel response = accountantService.UpdateAccountant(accountantBaseData);
                Log.Information("Accountant put accountantBaseData output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups put accountantBaseData error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="accountantId"></param>        
        [HttpDelete("{accountantId}")]
        public ResponseViewModel Delete(string accountantId)
        {
            try
            {
                Log.Information("Accountant delete(hide) accountantBaseData input {@Input}", accountantId);
                ResponseViewModel response = accountantService.DeleteAccountant(accountantId);
                Log.Information("Accountant delete(hide) accountantBaseData output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            { 
                Log.Error("AccountantGroups delete(hide) accountantBaseData error {@Error}", ex); 
                return ResponseUtil.InternalServerError(); 
            }
        }
    }
}

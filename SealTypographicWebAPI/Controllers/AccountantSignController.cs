using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using SealTypographicWebAPI.Util;
using Serilog;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 會計師印鑑組
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AccountantSignController : ControllerBase
    {
        /// <summary>
        /// 宣告會計師的interface
        /// </summary>
        protected readonly IAccountantSignService accountantSignService;

        /// <summary>
        /// 注入會計師interface
        /// </summary>
        /// <param name="accountantSignService"></param>        
        public AccountantSignController(IAccountantSignService accountantSignService)
        {
            this.accountantSignService = accountantSignService;            
        }

        /// <summary>
        /// 取得會計師簽名印鑑組
        /// </summary>
        /// <param name="accountantID">會計ID</param>        
        /// <returns></returns>
        [HttpGet("{accountantID}")]
        public AccountantSignViewModels Get(int accountantID)
        {
            AccountantSignViewModels accountantSignViewModels = new ();
            try
            {
                Log.Information("AccountantSign get{accountantID} input {@Input}", accountantID);
                accountantSignViewModels = accountantSignService.GetAccountantSings(accountantID);
                Log.Information("AccountantSign get{accountantID} output {@Output}", accountantSignViewModels);
                return accountantSignViewModels;                
            }
            catch (Exception ex)
            {             
                Log.Error("AccountantSign get{accountantID} error {@Error}", ex);
                accountantSignViewModels.DbError();
                return accountantSignViewModels;
            }
        }

        /// <summary>
        /// 建立會計師印鑑簽名組
        /// </summary>
        /// <param name="accountantSignPosts"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseViewModel Post(List<AccountantSignForm> accountantSignPosts)
        {
            ResponseViewModel response = new ();
            try
            {
                Log.Information("AccountantSign post input {@Input}", accountantSignPosts);
                response = accountantSignService.CreateAccountantSigns(accountantSignPosts);
                Log.Information("AccountantSign post output {@Output}", response);
                return response;                
            }
            catch (Exception ex)
            {                
                Log.Error("AccountantSign post error {@Error}", ex);
                response.DbError();
                return response;
            }
        }
        
        /// <summary>
        /// 修改簽名印鑑
        /// </summary>
        /// <param name="accountantSignUpdates">簽名印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public ResponseViewModel Put(List<AccountantSignFormUpdate> accountantSignUpdates)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSign put input {@Input}", accountantSignUpdates);
                response = accountantSignService.UpdateAccountantSigns(accountantSignUpdates);
                Log.Information("AccountantSign put output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {                
                Log.Error("AccountantSign put error {@Error}", ex);
                response.DbError();
                return response;
            }
        }

        /// <summary>
        /// 刪除會計師簽印，(隱藏)
        /// </summary>
        /// <param name="accountantSignId"></param>
        /// <returns></returns>
        [HttpDelete("{accountantSignId}")]
        public ResponseViewModel Delete(int accountantSignId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal delete input {@Input}", accountantSignId);
                response = accountantSignService.DeleteAccountantSign(accountantSignId);
                Log.Information("CustomerSeal delete output {@Ouput}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Customer delete error {@Error}", ex);
                response.DbError();
                return response;
            }
        }
    }
}

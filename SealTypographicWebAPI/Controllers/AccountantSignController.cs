using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;
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
        /// 取得會計師簽名啟用時間列
        /// </summary>
        /// <param name="accountantID">會計ID</param>        
        /// <returns></returns>
        [HttpGet("{accountantID}")]
        public AccountantSignGroupCreateDates Get(int accountantID)
        {
            AccountantSignGroupCreateDates accountantSignStartDates = new ();
            try
            {
                Log.Information("AccountantSign get{accountantID} input {@Input}", accountantID);
                accountantSignStartDates = accountantSignService.GetAccountantWithGruopCreateDate(accountantID);
                Log.Information("AccountantSign get{accountantID} output {@Output}", accountantSignStartDates);                        
            }
            catch (Exception ex)
            {             
                Log.Error("AccountantSign get{accountantID} error {@Error}", ex);
                accountantSignStartDates.DbError();                
            }
            return accountantSignStartDates;
        }

        /// <summary>
        /// 取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignStartDate">關鑑字</param>        
        /// <returns></returns>        
        [HttpGet]
        public AccountantSignViewModels Get([FromQuery] AccountantSignGroupCreateDate accountantSignStartDate)
        {
            AccountantSignViewModels accountantSignViewModels = new();
            try
            {
                Log.Information("AccountantSign get[FromQuery] input {@Input}", accountantSignStartDate);
                accountantSignViewModels = accountantSignService.GetAccountantSings(accountantSignStartDate);
                Log.Information("AccountantSign get[FromQuery] output {@Output}", accountantSignViewModels);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get error {@Error}", ex);
                accountantSignViewModels.DbError();                
            }
            return accountantSignViewModels;
        }

        /// <summary>
        /// 建立會計師印鑑簽印組
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
            }
            catch (Exception ex)
            {                
                Log.Error("AccountantSign post error {@Error}", ex);
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 將暫存的簽印組變更為待審
        /// </summary>
        /// <param name="accountantSignIds"></param>        
        /// <returns></returns>
        [HttpPut]
        public List<ResponseViewModel> Put(List<int> accountantSignIds)
        {
            List<ResponseViewModel> responseViewModels = new();
            try
            {
                Log.Information("AccountantSign put input {@Input}", accountantSignIds);
                responseViewModels = accountantSignService.UpdateReviewStatusPendingAccountantSigns(accountantSignIds);
                Log.Information("AccountantSign put output {@Output}", responseViewModels);                
            }
            catch (Exception ex)
            {                
                Log.Error("AccountantSign put error {@Error}", ex);
                ResponseViewModel response = new();
                response.DbError();
                responseViewModels.Add(response);
            }
            return responseViewModels;
        }

        /// <summary>
        /// 刪除會計師簽印，(作廢)
        /// </summary>
        /// <param name="accountantSignIds"></param>
        /// <returns></returns>
        [HttpDelete]
        public List<ResponseViewModel> Delete(List<int> accountantSignIds)
        {
            List<ResponseViewModel> responseViewModels = new();
            try
            {
                Log.Information("CustomerSeal delete input {@Input}", accountantSignIds);
                responseViewModels = accountantSignService.DeleteAccountantSign(accountantSignIds);
                Log.Information("CustomerSeal delete output {@Ouput}", responseViewModels);                
            }
            catch (Exception ex)
            {
                Log.Error("Customer delete error {@Error}", ex);
                ResponseViewModel response = new();
                response.DbError();
                responseViewModels.Add(response);                
            }
            return responseViewModels;
        }
    }
}

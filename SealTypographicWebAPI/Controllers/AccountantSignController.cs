using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Services;
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
        [HttpGet("{id}")]
        public AccountantSignViewModels Get(string accountantID)
        {
            try
            {
                Log.Information("AccountantSign Get accountantSignViewModel input {@Input}", accountantID);
                AccountantSignViewModels accountantSignViewModels = accountantSignService.GetAccountantSings(accountantID);
                Log.Information("AccountantSign Get accountantSignViewModel output {@Output}", accountantSignViewModels);
                return accountantSignViewModels;                
            }
            catch (Exception ex)
            {
                ResponseViewModel response = ResponseUtil.InternalServerError();
                Log.Error("AccountantSign Get accountantSignViewModel error {@Error}", ex);
                return new()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 建立會計師印鑑簽名組
        /// </summary>
        /// <param name="accountantSignPosts"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseViewModel Post(List<AccountantSignPost> accountantSignPosts)
        {
            try
            {
                Log.Information("AccountantSign post accountantSignDatas input {@Input}", accountantSignPosts);
                ResponseViewModel response = accountantSignService.CreateAccountantSigns(accountantSignPosts);
                Log.Information("AccountantSign post accountantSignDatas output {@Output}", response);
                return response;                
            }
            catch (Exception ex)
            {
                ResponseViewModel response = ResponseUtil.InternalServerError();
                Log.Error("AccountantSign post accountantSignDatas error {@Error}", ex);
                return new()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }
        
        /// <summary>
        /// 修改簽名印鑑
        /// </summary>
        /// <param name="accountantSignUpdates">簽名印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public ResponseViewModel Put(List<AccountantSignUpdate> accountantSignUpdates)
        {
            try
            {
                Log.Information("AccountantSign put accountantSignDatas input {@Input}", accountantSignUpdates);
                ResponseViewModel response = accountantSignService.UpdateAccountantSigns(accountantSignUpdates);
                Log.Information("AccountantSign put accountantSignDatas output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                ResponseViewModel response = ResponseUtil.InternalServerError();
                Log.Error("AccountantSign put accountantSignDatas error {@Error}", ex);
                return new()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 刪除印鑑(變更不啟用狀態)
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

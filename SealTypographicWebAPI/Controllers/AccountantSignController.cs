using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Accountant;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 會計師印鑑組
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountantSignController : ControllerBase
    {
        /// <summary>
        /// 宣告會計師的interface
        /// </summary>
        protected readonly IAccountantService accountantService;

        /// <summary>
        /// 回應結果
        /// </summary>
        protected readonly ResponseService responseService;

        /// <summary>
        /// 注入會計師interface
        /// </summary>
        /// <param name="accountantService"></param>
        /// <param name="responseService"></param>
        public AccountantSignController(IAccountantService accountantService, ResponseService responseService)
        {
            this.accountantService = accountantService;
            this.responseService = responseService;
        }

        /// <summary>
        /// 取得會計師簽名印鑑組
        /// </summary>
        /// <param name="accountantID">會計ID</param>        
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(string accountantID)
        {
            try
            {
                //List<AccountantSign> customerSeals = accountantService.GetAccountantSigns(accountantID);
                return Ok();
            }
            catch
            {
                return NotFound(responseService.Get(ResponseCode.InternalServerError));
            }
        }

        /// <summary>
        /// 建立會計師印鑑簽名組
        /// </summary>
        /// <param name="accountantSigns"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(List<AccountantSign> accountantSigns)
        {
            try
            {
                return Ok("OK");
            }
            catch
            {
                return NotFound(responseService.Get(ResponseCode.InternalServerError));
            }
        }
        
        /// <summary>
        /// 修改簽名印鑑
        /// </summary>
        /// <param name="accountantSign">簽名印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(List<AccountantSignWithId> accountantSign)
        {
            try
            {
                return Ok("OK");
            }
            catch
            {
                return NotFound(responseService.Get(ResponseCode.InternalServerError));
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

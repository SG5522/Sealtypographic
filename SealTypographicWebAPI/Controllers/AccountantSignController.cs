using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Service;
using SealTypographicWebAPI.Service.Accountant;
using SealTypographicWebAPI.Service.Customer;

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
        /// 注入會計處理函式
        /// </summary>
        protected Accountant accountant = new(new DeloitteAccount());

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        protected ErrorMessage errorMessage = new();

        /// <summary>
        /// 取得會計師簽名印鑑組
        /// </summary>
        /// <param name="accountantID">會計ID</param>        
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int accountantID)
        {
            try
            {
                List<AccountantSign> customerSeals = accountant.GetAccountantSigns(accountantID);
                return Ok(customerSeals);
            }
            catch
            {
                return NotFound(JsonConvert.SerializeObject(errorMessage.Get()));
            }
        }

        /// <summary>
        /// 
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
                return NotFound(JsonConvert.SerializeObject(errorMessage.Get()));
            }
        }
        
        /// <summary>
        /// 修改簽名印鑑
        /// </summary>
        /// <param name="accountantSign">簽名印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(List<AccountantSignAddID> accountantSign)
        {
            try
            {
                return Ok("OK");
            }
            catch
            {
                return NotFound(JsonConvert.SerializeObject(errorMessage.Get()));
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

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Service.Customer;

namespace SealTypographicWebAPI.Controllers
{    
    /// <summary>
    /// 客戶章
    /// </summary>
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CustomerSealController : ControllerBase
    {
        /// <summary>
        /// 注入顧客處理函式
        /// </summary>
        protected Customer customer = new(new DeloitteCustomer());

        /// <summary>
        /// 取得印鑑
        /// </summary>
        /// <param name="customerID">顧客ID(顧表表ID 非勤業自行定義的六碼英數字)</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>        
        [HttpGet("{customerID}/{quarter}")]
        public IActionResult Get(int customerID, string quarter)
        {
            try
            {
                List<CustomerSeal> customerSeals = customer.GetcustomerSeals(customerID, quarter);

                //return Ok(JsonConvert.SerializeObject(customerSeals));
                return Ok(customerSeals);
            }
            catch
            {
                var json = new
                {
                    message = "Error",
                    status = 0
                };
                //return JsonConvert.SerializeObject(json);
                return NotFound(JsonConvert.SerializeObject(json));
            }
        }
        /// <summary>
        /// 建立客戶資料
        /// </summary>        
        /// <param name="customerSeals">客戶印鑑組資料(Json)</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(List<CustomerSeal> customerSeals)
        {
            try
            {
                return Ok("OK");
            }
            catch
            {
                var json = new
                {
                    message = "Error",
                    status = 0
                };
                return NotFound(JsonConvert.SerializeObject(json));
            }
        }
        /// <summary>
        /// 修改印鑑
        /// </summary>        
        /// <param name="customerSealAddIDs">印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(List<CustomerSealAddID> customerSealAddIDs)
        {
            try
            {
                return Ok("OK");
            }
            catch
            {
                var json = new
                {
                    message = "Error",
                    status = 0
                };
                return NotFound(JsonConvert.SerializeObject(json));
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

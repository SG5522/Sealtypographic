using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using Newtonsoft.Json;
using SealTypographicWebAPI.Service.Customer;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 顧客基本資料
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class CustomerDataController : ControllerBase
    {
        /// <summary>
        /// 注入顧客處理函式
        /// </summary>
        protected Customer customer = new(new DeloitteCustomer());

        /// <summary>
        /// /// 取得顧客基本資料
        /// </summary>
        /// <param name="id">顧客ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                CustomerData customerData = customer.GetCustomerData(id);
                return Ok(customerData);
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
        /// 建立顧客基本資料
        /// </summary>
        /// <param name="customerData">基本資料</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(CustomerData customerData)
        {
            try
            {                
                return Ok(customerData);
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
        /// 更新基本資料
        /// </summary>
        /// <param name="customerDataAddID">基本資料</param>
        [HttpPut]
        public IActionResult Put(CustomerDataAddID customerDataAddID)
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
    }
}

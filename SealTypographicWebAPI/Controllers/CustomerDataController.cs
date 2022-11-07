using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using Newtonsoft.Json;
using SealTypographicWebAPI.Service;
using SealTypographicWebAPI.Service.Customer;
using System.Globalization;

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
        protected Customer customer = new(new CustomerDeloitte());

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        protected ErrorMessage errorMessage = new();

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>        
        /// <param name="customerIDOrName"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCustomerList(string customerIDOrName)
        {            
            try
            {
                List<CustomerListData> customerListData = customer.GetCustomerList(customerIDOrName);
                return Ok(customerListData);
            }
            catch
            {
                return NotFound(JsonConvert.SerializeObject(errorMessage.Get()));
            }
        }


        /// <summary>
        /// /// 取得顧客基本資料
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(string customerID)
        {
            try
            {
                CustomerData customerData = customer.GetCustomerData(customerID);
                return Ok(customerData);
            }
            catch
            {
                return NotFound(JsonConvert.SerializeObject(errorMessage.Get()));
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
                return NotFound(JsonConvert.SerializeObject(errorMessage.Get()));
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
                return NotFound(JsonConvert.SerializeObject(errorMessage.Get()));
            }
        }
    }
}

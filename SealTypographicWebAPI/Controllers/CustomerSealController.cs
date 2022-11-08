using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Customer;

namespace SealTypographicWebAPI.Controllers
{    
    /// <summary>
    /// 客戶章
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CustomerSealController : ControllerBase
    {
        /// <summary>
        /// 宣告顧客資料處理的interface
        /// </summary>
        protected readonly ICustomerService customerService;

        /// <summary>
        /// 注入顧客interface
        /// </summary>
        /// <param name="customerService"></param>
        public CustomerSealController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        protected ResponseService responseService = new();

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerID">顧客ID(顧表表ID 非勤業自行定義的六碼英數字)</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>        
        [HttpGet("{customerID}/{quarter}")]
        public CustomerSeals Get(string customerID, string quarter)
        {
            try
            {
                return customerService.GetcustomerSeals(customerID, quarter);
            }
            catch
            {
                Response response = responseService.Get();
                return new CustomerSeals()
                {
                    ResponseStatus = response.ResponseStatus,
                    ResponseMessage = response.ResponseMessage,
                };
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
                return NotFound(responseService.Get());
            }
        }
        /// <summary>
        /// 修改印鑑
        /// </summary>        
        /// <param name="customerSealAddIDs">印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(List<CustomerSealWithId> customerSealAddIDs)
        {
            try
            {
                return Ok("OK");
            }
            catch
            {
                return NotFound(responseService.Get());
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

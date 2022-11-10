using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;
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
        protected readonly ICustomerSealService customerSealService;

        /// <summary>
        /// 注入顧客interface
        /// </summary>
        /// <param name="customerSealService"></param>
        public CustomerSealController(ICustomerSealService customerSealService)
        {
            this.customerSealService = customerSealService;
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
                return customerSealService.GetCustomerSeals(customerID, quarter);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
                return new CustomerSeals()
                {
                    Code = response.Code,
                    Message = response.Message,
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
                return NotFound(responseService.Get(ResponseCode.InternalServerError));
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

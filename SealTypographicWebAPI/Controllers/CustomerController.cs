using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;
using Newtonsoft.Json;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Customer;
using System.Globalization;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 顧客基本資料
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {        
        /// <summary>
        /// 宣告顧客資料處理的interface
        /// </summary>
        protected readonly ICustomerService customerService;

        /// <summary>
        /// 注入顧客interface
        /// </summary>
        /// <param name="customerService"></param>
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;            
        }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        protected ResponseService responseService = new();

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>        
        /// <param name="customerIDOrName"></param>
        /// <returns></returns>
        [HttpGet]
        public CustomerResponseViewModel GetCustomerViewModels(string customerIDOrName)
        {            
            try
            {                
                return customerService.GetCustomerViewModels(customerIDOrName);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
                return new CustomerResponseViewModel()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }


        /// <summary>
        /// 取得顧客基本資料
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Customer Get(string customerID)
        {
            try
            {                                
                return customerService.GetCustomer(customerID);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
                return new Customer()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }
        /// <summary>
        /// 建立顧客基本資料
        /// </summary>
        /// <param name="customerData">基本資料</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(Customer customerData)
        {
            try
            {                
                customerService.CreateCustomer(customerData);                
                return Ok();
            }
            catch
            {
                return NotFound(responseService.Get(ResponseCode.InternalServerError));
            }
        }

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="customerDataAddID">基本資料</param>
        [HttpPut]
        public IActionResult Put(CustomerWithId customerDataAddID)
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
    }
}

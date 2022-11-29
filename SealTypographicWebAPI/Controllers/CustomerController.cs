using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Services;
using Serilog;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理客戶基本資料
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CustomerController : ControllerBase
    {        
        /// <summary>
        /// 宣告顧客資料處理的interface
        /// </summary>
        protected readonly ICustomerService customerService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="customerService">管理客戶資料</param>
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;                        
        }

        /// <summary>
        /// 依搜尋條件獲得客戶資料列表
        /// </summary>
        /// <param name="customerQuery">客戶分頁搜尋</param>        
        /// <returns></returns>
        [HttpGet]        
        public CustomerResponsePage Get([FromQuery]CustomerSearch customerQuery)
        {         
            try
            {
                Log.Information("Customer get viewModels input {@Input}", customerQuery);
                CustomerResponsePage customerResponsePage = customerService.GetCustomerViewModels(customerQuery);
                Log.Information("Customer get viewModels output {@Output}", customerResponsePage);
                return customerResponsePage;
            }
            catch (Exception ex)
            {
                Log.Error("Customer get viewModels error {@Error}", ex);
                Response response = ResponseUtil.InternalServerError();                
                return new()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }


        /// <summary>
        /// 取得客戶基本資料
        /// </summary>
        /// <param name="id">顧客ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public CustomerResponse Get(string id)
        {
            try
            {
                Log.Information("Customer get customerData input {@Input}", id);
                CustomerResponse customerResponse = customerService.GetCustomer(id);
                Log.Information("Customer get customerData output {@Output}", customerResponse);
                return customerResponse;
            }
            catch (Exception ex)
            {
                Log.Error("Customer get customerData error {@Error}", ex);
                Response response = ResponseUtil.InternalServerError();
                return new CustomerResponse()
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
        public Response Post(CustomerData customerData)
        {
            try
            {
                Log.Information("Customer post customerData input {@Input}", customerData);
                Response response = customerService.CreateCustomer(customerData);
                Log.Information("Customer post customerData output {@Input}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Customer post customerData error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="customerData">基本資料</param>
        [HttpPut]
        public Response Put(CustomerData customerData)
        {
            try
            {
                Log.Information("Customer put customerData input {@Input}", customerData);
                Response response = customerService.UpdateCustomer(customerData);
                Log.Information("Customer put customerData output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Customer put customerData error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete("{customerId}")]
        public Response Delete(string customerId)
        {
            try
            {
                Log.Information("Customer delete customerData input {@Input}", customerId);
                Response response = customerService.DeleteCustomer(customerId);
                Log.Information("Customer delete customerData input {@Input}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Customer delete customerData error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }
    }
}

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
        public CustomerViewModelPaginate Get([FromQuery]CustomerSearch customerQuery)
        {         
            try
            {
                Log.Information("Customer get viewModels input {@Input}", customerQuery);
                CustomerViewModelPaginate customerResponsePage = customerService.GetCustomerViewModels(customerQuery);
                Log.Information("Customer get viewModels output {@Output}", customerResponsePage);
                return customerResponsePage;
            }
            catch (Exception ex)
            {
                Log.Error("Customer get viewModels error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.InternalServerError();                
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
        public CustomerResponseViewModel Get(string id)
        {
            try
            {
                Log.Information("Customer get customerForm input {@Input}", id);
                CustomerResponseViewModel customerResponse = customerService.GetCustomer(id);
                Log.Information("Customer get customerForm output {@Output}", customerResponse);
                return customerResponse;
            }
            catch (Exception ex)
            {
                Log.Error("Customer get customerForm error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.InternalServerError();
                return new CustomerResponseViewModel()
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
        public ResponseViewModel Post(CustomerForm customerData)
        {
            try
            {
                Log.Information("Customer post customerForm input {@Input}", customerData);
                ResponseViewModel response = customerService.CreateCustomer(customerData);
                Log.Information("Customer post customerForm output {@Input}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Customer post customerForm error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        [HttpPut]
        public ResponseViewModel Put(CustomerForm customerForm)
        {
            try
            {               
                Log.Information("Customer put customerForm input {@Input}", customerForm);
                ResponseViewModel response = customerService.UpdateCustomer(customerForm);
                Log.Information("Customer put customerForm output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Customer put customerForm error {@Error}", ex);
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
        public ResponseViewModel Delete(string customerId)
        {
            try
            {
                Log.Information("Customer delete customerForm input {@Input}", customerId);
                ResponseViewModel response = customerService.DeleteCustomer(customerId);
                Log.Information("Customer delete customerForm input {@Input}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Customer delete customerForm error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }
    }
}

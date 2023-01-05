using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
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
        public CustomerPaginateViewModel Get([FromQuery]CustomerSearch customerQuery)
        {
            CustomerPaginateViewModel customerResponsePage = new ();
            try
            {
                Log.Information("Customer get FromQuery input {@Input}", customerQuery);
                customerResponsePage = customerService.GetCustomerPaginatesViewModel(customerQuery);
                Log.Information("Customer get FromQuery output {@Output}", customerResponsePage);
            }
            catch (Exception ex)
            {
                Log.Error("Customer get FromQuery error {@Error}", ex);                
                customerResponsePage.DbError();                
            }
            return customerResponsePage;
        }


        /// <summary>
        /// 取得客戶基本資料
        /// </summary>
        /// <param name="customerId">顧客ID</param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        public CustomerDetailViewModel Get(int customerId)
        {
            CustomerDetailViewModel customerDetailViewModel = new();
            try
            {
                Log.Information("Customer get customerId input {@Input}", customerId);
                customerDetailViewModel = customerService.GetCustomerDetailViewModel(customerId);
                Log.Information("Customer get customerId output {@Output}", customerDetailViewModel);                
            }
            catch (Exception ex)
            {
                Log.Error("Customer get customerId error {@Error}", ex);                
                customerDetailViewModel.DbError();                
            }
            return customerDetailViewModel;
        }
        /// <summary>
        /// 建立顧客基本資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        /// <returns></returns>
        [HttpPost]
        public CreateCustomerResponse Post(CustomerForm customerForm)
        {
            CreateCustomerResponse createCustomerResponse = new();
            try
            {
                Log.Information("Customer post input {@Input}", customerForm);
                createCustomerResponse = customerService.CreateCustomer(customerForm);
                Log.Information("Customer post output {@Input}", createCustomerResponse);                
            }
            catch (Exception ex)
            {
                Log.Error("Customer post error {@Error}", ex);
                createCustomerResponse.DbError();                
            }
            return createCustomerResponse;
        }

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        [HttpPut]
        public ResponseViewModel Put(CustomerFormUpdate customerForm)
        {
            ResponseViewModel response = new();
            try
            {               
                Log.Information("Customer put input {@Input}", customerForm);
                response = customerService.UpdateCustomer(customerForm);
                Log.Information("Customer put output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("Customer put error {@Error}", ex);
                response.DbError();                
            }
            return response;
        }

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete("{customerId}")]
        public ResponseViewModel Delete(int customerId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("Customer delete input {@Input}", customerId);
                response = customerService.DeleteCustomer(customerId);
                Log.Information("Customer delete input {@Input}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("Customer delete error {@Error}", ex);
                response.DbError();
            }
            return response;
        }
    }
}

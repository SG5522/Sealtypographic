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
    [ApiController]
    public class CustomerController : ControllerBase
    {
        /// <summary>
        /// 管理客戶資料的Service
        /// </summary>
        private readonly ICustomerService customerService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="customerService">管理客戶資料的Service</param>
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;                        
        }

        /// <summary>
        /// 取得客戶資料列表(分頁)
        /// </summary>
        /// <param name="customerSearch">客戶分頁搜尋</param>        
        /// <returns></returns>
        [HttpGet]        
        public CustomerPaginateViewModel Paginate([FromQuery]CustomerSearch customerSearch)
        {
            CustomerPaginateViewModel customerPaginateViewModel = new ();
            try
            {
                Log.Information("CustomerSealAuthorization get paginate input {@Input}", customerSearch);
                customerPaginateViewModel = customerService.GetPaginate(customerSearch);
                Log.Information("CustomerSealAuthorization get paginate output {@Output}", customerPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealAuthorization get paginate error {@Error}", ex);                
                customerPaginateViewModel.DbError();                
            }
            return customerPaginateViewModel;
        }

        /// <summary>
        /// 取得客戶詳細基本資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        public CustomerDetailViewModel Detail(int customerId)
        {
            CustomerDetailViewModel customerDetailViewModel = new();
            try
            {
                Log.Information("CustomerSealAuthorization get detail input {@Input}", customerId);
                customerDetailViewModel = customerService.GetDetail(customerId);
                Log.Information("CustomerSealAuthorization get detail output {@Output}", customerDetailViewModel);                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealAuthorization get customerId error {@Error}", ex);                
                customerDetailViewModel.DbError();                
            }
            return customerDetailViewModel;
        }

        /// <summary>
        /// 新增客戶基本資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        /// <returns></returns>
        [HttpPost]
        public CreateCustomerResponse New(CustomerForm customerForm)
        {
            CreateCustomerResponse createCustomerResponse = new();
            try
            {
                Log.Information("CustomerSealAuthorization new input {@Input}", customerForm);
                createCustomerResponse = customerService.New(customerForm);
                Log.Information("CustomerSealAuthorization new output {@Input}", createCustomerResponse);                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealAuthorization new error {@Error}", ex);
                createCustomerResponse.DbError();                
            }
            return createCustomerResponse;
        }

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="customerUpdateForm">基本資料</param>
        [HttpPut]
        public ResponseViewModel Update(CustomerUpdateForm customerUpdateForm)
        {
            ResponseViewModel response = new();
            try
            {               
                Log.Information("CustomerSealAuthorization update input {@Input}", customerUpdateForm);
                response = customerService.Update(customerUpdateForm);
                Log.Information("CustomerSealAuthorization update output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealAuthorization update error {@Error}", ex);
                response.DbError();                
            }
            return response;
        }

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="customerId">客戶Id</param>
        /// <returns></returns>
        [HttpDelete("{customerId}")]
        public ResponseViewModel Delete(int customerId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSealAuthorization delete input {@Input}", customerId);
                response = customerService.Delete(customerId);
                Log.Information("CustomerSealAuthorization delete output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSealAuthorization delete error {@Error}", ex);
                response.DbError();
            }
            return response;
        }
    }
}

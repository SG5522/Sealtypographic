using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Customer;


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
        /// 回應結果
        /// </summary>
        protected readonly ResponseService responseService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="customerService">管理客戶資料</param>
        /// <param name="responseService">回傳結果</param>
        public CustomerController(ICustomerService customerService,ResponseService responseService)
        {
            this.customerService = customerService;            
            this.responseService = responseService;
        }

        /// <summary>
        /// 依搜尋條件獲得客戶資料列表
        /// </summary>
        /// <param name="customerQueryPage">客戶分頁搜尋</param>        
        /// <returns></returns>
        [HttpGet]
        //public CustomerResponsePage GetCustomerViewModels(string customerIDOrName, int thisPage, int pageSize)
        public CustomerResponsePage GetCustomerViewModels([FromQuery]CustomerQueryPage customerQueryPage)
        {         
            try
            {                
                return customerService.GetCustomerViewModels(customerQueryPage);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
                return new CustomerResponsePage()
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
                return customerService.GetCustomer(id);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
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
        public Response Post(CustomerBaseData customerData)
        {
            try
            {                                          
                return customerService.CreateCustomer(customerData);
            }
            catch
            {
                return responseService.Get(ResponseCode.InternalServerError);
            }
        }

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="customerData">基本資料</param>
        [HttpPut]
        public Response Put(CustomerBaseData customerData)
        {
            try
            {                
                return customerService.UpdateCustomer(customerData);
            }
            catch
            {
                return responseService.Get(ResponseCode.InternalServerError);
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
                return customerService.DeleteCustomer(customerId);
            }
            catch
            {
                return responseService.Get(ResponseCode.InternalServerError);
            }
        }
    }
}

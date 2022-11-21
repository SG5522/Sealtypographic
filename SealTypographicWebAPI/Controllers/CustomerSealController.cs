using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Customer;
using SealTypographicWebAPI.Models.Customer;

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
        /// 回傳結果
        /// </summary>
        protected readonly ResponseService responseService;

        /// <summary>
        /// 注入顧客interface
        /// </summary>
        /// <param name="customerSealService"></param>
        /// <param name="responseService">回傳結果</param>
        public CustomerSealController(ICustomerSealService customerSealService, ResponseService responseService)
        {
            this.customerSealService = customerSealService;
            this.responseService = responseService;
        }

        /// <summary>
        /// 取得顧客印鑑季度表
        /// </summary>
        /// <param name="customerId">顧客ID</param>
        /// <returns></returns>
        [HttpGet("customerId")]
        public CustomerSealQuarters Get(string customerId)
        {
            try
            {
                return customerSealService.GetCustomerSealQuarters(customerId);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
                return new CustomerSealQuarters()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarter">關鑑字</param>        
        /// <returns></returns>        
        [HttpGet]
        public CustomerSealViewModels Get([FromQuery]CustomerSealQuarter customerSealQuarter)
        {
            try
            {
                return customerSealService.GetCustomerSealViewModels(customerSealQuarter);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
                return new CustomerSealViewModels()
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
        public Response Post(List<CustomerSeal> customerSeals)
        {
            try
            {
                return customerSealService.CreateCustomerSeals(customerSeals);
            }
            catch
            {
                return responseService.Get(ResponseCode.InternalServerError);
            }
        }
        /// <summary>
        /// 修改印鑑
        /// </summary>        
        /// <param name="customerSealAddIDs">印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public Response Put(List<CustomerSealPostData> customerSealAddIDs)
        {
            try
            {
                return customerSealService.UpdateCustomerSeals(customerSealAddIDs);
            }
            catch
            {
                return responseService.Get(ResponseCode.InternalServerError);
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

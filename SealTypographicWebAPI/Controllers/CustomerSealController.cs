using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Services;
using Serilog;

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
        /// 取得顧客印鑑季度表
        /// </summary>
        /// <param name="customerId">顧客ID</param>
        /// <returns></returns>
        [HttpGet("customerId")]
        public CustomerSealQuarters Get(string customerId)
        {
            try
            {
                Log.Information("CustomerSeal get customerSealQuarters input {@Input}", customerId);
                CustomerSealQuarters customerSealQuarters = customerSealService.GetCustomerSealQuarters(customerId);
                Log.Information("CustomerSeal get customerSealQuarters output {@Output}", customerSealQuarters);
                return customerSealQuarters;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get customerSealQuarters error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.InternalServerError();
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
                Log.Information("CustomerSeal get customerSealViewModels input {@Input}", customerSealQuarter);
                CustomerSealViewModels customerSealViewModels = customerSealService.GetCustomerSealViewModels(customerSealQuarter);
                Log.Information("CustomerSeal get customerSealViewModels output {@Output}", customerSealViewModels);
                return customerSealViewModels;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get customerSealViewModels error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.InternalServerError();
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
        public ResponseViewModel Post(List<CustomerSealForm> customerSeals)
        {
            try
            {
                Log.Information("CustomerSeal post customerSealData input {@Input}", customerSeals);
                ResponseViewModel response = customerSealService.CreateCustomerSeals(customerSeals);
                Log.Information("CustomerSeal post customerSealData output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal post customerSealData error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }
        /// <summary>
        /// 修改印鑑
        /// </summary>        
        /// <param name="customerSealPostDatas">印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public ResponseViewModel Put(List<CustomerSealFormWithID> customerSealPostDatas)
        {
            try
            {
                Log.Information("CustomerSeal post customerSealData input {@Input}", customerSealPostDatas);
                ResponseViewModel response = customerSealService.UpdateCustomerSeals(customerSealPostDatas);
                Log.Information("CustomerSeal post customerSealData output {@Output}", response);
                return response;                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal put customerSealData error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }
    }
}

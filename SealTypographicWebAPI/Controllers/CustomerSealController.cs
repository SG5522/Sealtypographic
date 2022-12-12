using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Services;
using Serilog;
using SealTypographicWebAPI.Services.Implements;

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
        public CustomerSealQuarters Get(int customerId)
        {
            try
            {
                Log.Information("CustomerSeal get input {@Input}", customerId);
                CustomerSealQuarters customerSealQuarters = customerSealService.GetCustomerSealQuarters(customerId);
                Log.Information("CustomerSeal get output {@Output}", customerSealQuarters);
                return customerSealQuarters;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
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
                Log.Information("CustomerSeal get[FromQuery] input {@Input}", customerSealQuarter);
                CustomerSealViewModels customerSealViewModels = customerSealService.GetCustomerSealViewModels(customerSealQuarter);
                Log.Information("CustomerSeal get[FromQuery] output {@Output}", customerSealViewModels);
                return customerSealViewModels;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
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
                Log.Information("CustomerSeal post input {@Input}", customerSeals);
                ResponseViewModel response = customerSealService.CreateCustomerSeals(customerSeals);
                Log.Information("CustomerSeal post output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal post error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }

        /// <summary>
        /// 修改印鑑
        /// </summary>        
        /// <param name="customerSealPostDatas">印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public ResponseViewModel Put(List<CustomerSealFormUpdate> customerSealPostDatas)
        {
            try
            {
                Log.Information("CustomerSeal put input {@Input}", customerSealPostDatas);
                ResponseViewModel response = customerSealService.UpdateCustomerSeals(customerSealPostDatas);
                Log.Information("CustomerSeal put output {@Output}", response);
                return response;                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal put error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }

        /// <summary>
        /// 刪除印鑑，(隱藏)
        /// </summary>
        /// <param name="customerSealId"></param>
        /// <returns></returns>
        [HttpDelete("{customerSealId}")]
        public ResponseViewModel Delete(int customerSealId)
        {
            try
            {
                Log.Information("CustomerSeal delete input {@Input}", customerSealId);
                ResponseViewModel response = customerSealService.DeleteCustomerSeal(customerSealId);
                Log.Information("CustomerSeal delete output {@Ouput}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Customer delete customerForm error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }
    }
}

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
        [HttpGet("{customerId}")]
        public CustomerSealQuarters Get(int customerId)
        {
            CustomerSealQuarters customerSealQuarters = new ();
            try
            {
                Log.Information("CustomerSeal get{customerId} input {@Input}", customerId);
                customerSealQuarters = customerSealService.GetCustomerSealQuarters(customerId);
                Log.Information("CustomerSeal get{customerId} output {@Output}", customerSealQuarters);
                return customerSealQuarters;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get{customerId} error {@Error}", ex);                
                customerSealQuarters.DbError();
                return customerSealQuarters;
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
            CustomerSealViewModels customerSealViewModels = new ();
            try
            {
                Log.Information("CustomerSeal get[FromQuery] input {@Input}", customerSealQuarter);
                customerSealViewModels = customerSealService.GetCustomerSealViewModels(customerSealQuarter);
                Log.Information("CustomerSeal get[FromQuery] output {@Output}", customerSealViewModels);
                return customerSealViewModels;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get error {@Error}", ex);                
                customerSealViewModels.DbError();
                return customerSealViewModels;
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
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal post input {@Input}", customerSeals);
                response = customerSealService.CreateCustomerSeals(customerSeals);
                Log.Information("CustomerSeal post output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal post error {@Error}", ex);
                response.DbError();
                return response;
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
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal put input {@Input}", customerSealPostDatas);
                response = customerSealService.UpdateCustomerSeals(customerSealPostDatas);
                Log.Information("CustomerSeal put output {@Output}", response);
                return response;                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal put error {@Error}", ex);
                response.DbError();
                return response;
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
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal delete input {@Input}", customerSealId);
                response = customerSealService.DeleteCustomerSeal(customerSealId);
                Log.Information("CustomerSeal delete output {@Ouput}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Customer delete error {@Error}", ex);
                response.DbError();
                return response;
            }
        }
    }
}

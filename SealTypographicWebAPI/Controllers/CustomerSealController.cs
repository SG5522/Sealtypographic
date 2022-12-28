using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
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
        [HttpGet("{customerId}")]
        public CustomerSealQuarterViews Get(int customerId)
        {
            CustomerSealQuarterViews customerSealQuarters = new ();
            try
            {
                Log.Information("CustomerSeal get{customerId} input {@Input}", customerId);
                customerSealQuarters = customerSealService.GetCustomerSealQuarters(customerId);
                Log.Information("CustomerSeal get{customerId} output {@Output}", customerSealQuarters);                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get{customerId} error {@Error}", ex);                
                customerSealQuarters.DbError();                
            }
            return customerSealQuarters;
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarter">關鑑字</param>        
        /// <returns></returns>        
        [HttpGet]
        public CustomerSealViewModels Get([FromQuery] CustomerSealQuarterSearch customerSealQuarter)
        {
            CustomerSealViewModels customerSealViewModels = new();
            try
            {
                Log.Information("CustomerSeal get[FromQuery] input {@Input}", customerSealQuarter);
                customerSealViewModels = customerSealService.GetCustomerSealViewModels(customerSealQuarter);
                Log.Information("CustomerSeal get[FromQuery] output {@Output}", customerSealViewModels);                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get error {@Error}", ex);                
                customerSealViewModels.DbError();                
            }
            return customerSealViewModels;
        }        

        /// <summary>
        /// 建立客戶印鑑組資料
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
                
            }
            catch (Exception ex)
            {                
                Log.Error("CustomerSeal post error {@Error}", ex);
                response.DbError();                                
            }
            return response;
        }

        /// <summary>
        /// 異動客戶印鑑
        /// </summary>        
        /// <param name="customerSealUpdate">刪除修改新增的list</param>
        /// <returns></returns>
        [HttpPut]
        public List<ResponseViewModel> Put(CustomerSealUpdate customerSealUpdate)
        {            
            List<ResponseViewModel> responses = new();
            try
            {
                Log.Information("CustomerSeal put input {@Input}", customerSealUpdate);                
                responses = customerSealService.UpdateCustomerSeals(customerSealUpdate);
                Log.Information("CustomerSeal put output {@Output}", responses);                               
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal put error {@Error}", ex);
                ResponseViewModel responseViewModel = new();
                responseViewModel.DbError();
                responses.Add(responseViewModel);                
            }
            return responses;
        }

        /// <summary>
        /// 變更此季度印鑑待審
        /// </summary>        
        /// <param name="customerSealQuarter">客戶Id與季度</param>
        /// <returns></returns>
        [HttpPut("Pending")]
        public ResponseViewModel PutPendingCustomerSeal(CustomerSealQuarterSearch customerSealQuarter)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal put input {@Input}", customerSealQuarter);
                response = customerSealService.PendingCustomerSeal(customerSealQuarter);
                Log.Information("CustomerSeal put output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal put error {@Error}", ex);
                response.DbError();                                
            }
            return response;
        }

        /// <summary>
        /// 變更此季度印鑑作廢
        /// </summary>        
        /// <param name="customerSealQuarter">客戶Id與季度</param>
        /// <returns></returns>
        [HttpPut("Invalid")]
        public ResponseViewModel PutInvalidCustomerSeal(CustomerSealQuarterSearch customerSealQuarter)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal put input {@Input}", customerSealQuarter);
                response = customerSealService.InvalidCustomerSeal(customerSealQuarter);
                Log.Information("CustomerSeal put output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal put error {@Error}", ex);
                response.DbError();                
            }
            return response;
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

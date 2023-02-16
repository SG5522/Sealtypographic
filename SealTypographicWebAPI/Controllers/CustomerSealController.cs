using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Services;
using Serilog;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理客戶印鑑
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSealController : ControllerBase
    {
        /// <summary>
        /// 客戶印鑑管理的service
        /// </summary>
        private readonly ICustomerSealService customerSealService;

        /// <summary>
        /// 建構:注入客戶印鑑管理的service
        /// </summary>
        /// <param name="customerSealService">客戶印鑑管理的service</param>        
        public CustomerSealController(ICustomerSealService customerSealService)
        {
            this.customerSealService = customerSealService;
        }

        /// <summary>
        /// 取得客戶印鑑季度表
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        public CustomerSealQuarterResponse Quarter(int customerId)
        {
            CustomerSealQuarterResponse customerSealQuarters = new();
            try
            {
                Log.Information("CustomerSeal get quarter input {@Input}", customerId);
                customerSealQuarters = customerSealService.GetQuarter(customerId);
                Log.Information("CustomerSeal get quarter output {@Output}", customerSealQuarters);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get quarter error {@Error}", ex);
                customerSealQuarters.DbError();
            }
            return customerSealQuarters;
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>             
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public CustomerSealViewModels Seals(int customerSealQuarterId)
        {
            CustomerSealViewModels customerSealViewModels = new();
            try
            {
                Log.Information("CustomerSeal get seals input {@Input}", customerSealQuarterId);
                customerSealViewModels = customerSealService.GetSeals(customerSealQuarterId);
                Log.Information("CustomerSeal get seals output {@Output}", customerSealViewModels);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get seals error {@Error}", ex);
                customerSealViewModels.DbError();
            }
            return customerSealViewModels;
        }

        /// <summary>
        /// 新增客戶印鑑組資料
        /// </summary>        
        /// <param name="customerSealForms">客戶印鑑組資料</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseViewModel New(CustomerSealForm customerSealForms)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal new input {@Input}", customerSealForms);
                response = customerSealService.New(customerSealForms);
                Log.Information("CustomerSeal new output {@Output}", response);

            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal new error {@Error}", ex);
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 異動客戶印鑑
        /// </summary>        
        /// <param name="customerSealUpdate">需要異動客戶印鑑資料</param>
        /// <returns></returns>
        [HttpPut]
        public List<ResponseViewModel> Update(CustomerSealUpdate customerSealUpdate)
        {
            List<ResponseViewModel> responses = new();
            try
            {
                Log.Information("CustomerSeal update input {@Input}", customerSealUpdate);
                responses = customerSealService.Update(customerSealUpdate);
                Log.Information("CustomerSeal update output {@Output}", responses);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal update error {@Error}", ex);
                ResponseViewModel responseViewModel = new();
                responseViewModel.DbError();
                responses.Add(responseViewModel);
            }
            return responses;
        }

        /// <summary>
        /// 此季度印鑑從草稿狀態變更為待審
        /// </summary>        
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel Pending(int customerSealQuarterId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal pending input {@Input}", customerSealQuarterId);
                response = customerSealService.Pending(customerSealQuarterId);
                Log.Information("CustomerSeal pending output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal pending error {@Error}", ex);
                response.DbError();                                
            }
            return response;
        }

        /// <summary>
        /// 此季度印鑑從草稿狀態變更為作廢
        /// </summary>        
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel Invalid(int customerSealQuarterId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal invalid input {@Input}", customerSealQuarterId);
                response = customerSealService.Invalid(customerSealQuarterId);
                Log.Information("CustomerSeal invalid output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal invalid error {@Error}", ex);
                response.DbError();                
            }
            return response;
        }

        /// <summary>
        /// 此季度印鑑從待審狀態變更為草稿(收回)
        /// </summary>        
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel CancelReview(int customerSealQuarterId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal CancelReview input {@Input}", customerSealQuarterId);
                response = customerSealService.CancelReview(customerSealQuarterId);
                Log.Information("CustomerSeal CancelReview output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal CancelReview error {@Error}", ex);
                response.DbError();
            }
            return response;
        }
    }
}

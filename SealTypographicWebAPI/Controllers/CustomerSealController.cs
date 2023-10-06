using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.CustomerSeal;
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
        /// <param name="customerSealQuarterPaginateSearch">印鑑季度分頁搜尋</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealQuarterPaginateViewModel Quarter([FromQuery] CustomerSealQuarterPaginateSearch customerSealQuarterPaginateSearch)
        {
            CustomerSealQuarterPaginateViewModel customerSealQuarterPaginateViewModel = new();
            try
            {
                Log.Information("CustomerSeal get Quarter input {@Input}", customerSealQuarterPaginateSearch);
                customerSealQuarterPaginateViewModel = customerSealService.GetQuarter(customerSealQuarterPaginateSearch, false);
                Log.Information("CustomerSeal get Quarter output {@Output}", customerSealQuarterPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get Quarter error {@Error}", ex.Message);
                customerSealQuarterPaginateViewModel.Error();
                customerSealQuarterPaginateViewModel.Message = ex.Message;
            }
            return customerSealQuarterPaginateViewModel;
        }

        /// <summary>
        /// 取得客戶印鑑群組簡短訊息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="quarterId"></param>        
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealGroupResponse CustomerSealGroupSummary(int customerId, int quarterId)
        {
            CustomerSealGroupResponse customerSealGroupResponse = new();
            try
            {
                Log.Information("CustomerSeal get CustomerSealGroupSummary input {@Input1} {@Input2}", customerId, quarterId);
                customerSealGroupResponse = customerSealService.GetCustomerSealGroupSummry(customerId, quarterId);
                Log.Information("CustomerSeal get CustomerSealGroupSummary output {@Output}", customerSealGroupResponse);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get CustomerSealGroupSummary error {@Error}", ex.Message);
                customerSealGroupResponse.Error();
                customerSealGroupResponse.Message = ex.Message;
            }
            return customerSealGroupResponse;
        }


        /// <summary>
        /// 取得客戶印鑑季度表
        /// </summary>
        /// <param name="customerSealQuarterPaginateSearch">印鑑季度分頁搜尋</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealQuarterPaginateViewModel QuarterWithTypographic([FromQuery] CustomerSealQuarterPaginateSearch customerSealQuarterPaginateSearch)
        {
            CustomerSealQuarterPaginateViewModel customerSealQuarterPaginateViewModel = new();
            try
            {
                Log.Information("CustomerSeal get quarterWithTypographic input {@Input}", customerSealQuarterPaginateSearch);
                customerSealQuarterPaginateViewModel = customerSealService.GetQuarter(customerSealQuarterPaginateSearch, true);
                Log.Information("CustomerSeal get quarterWithTypographic output {@Output}", customerSealQuarterPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get quarterWithTypographic error {@Error}", ex.Message);
                customerSealQuarterPaginateViewModel.Error();
                customerSealQuarterPaginateViewModel.Message = ex.Message;
            }
            return customerSealQuarterPaginateViewModel;
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <param name="isTransparent" example="false">是否白底透明化處理</param>             
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public CustomerSealViewModels Seals(int customerSealQuarterId, bool isTransparent)
        {
            CustomerSealViewModels customerSealViewModels = new();
            try
            {
                Log.Information("CustomerSeal get seals input {@Input}", customerSealQuarterId);
                customerSealViewModels = customerSealService.GetSeals(customerSealQuarterId, isTransparent);
                Log.Information("CustomerSeal get seals output {@Output}", customerSealViewModels);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal get seals error {@Error}", ex.Message); 
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
        public async Task<ResponseViewModel> New(CustomerSealForm customerSealForms)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("CustomerSeal new input {@Input}", customerSealForms);
                response = await customerSealService.New(customerSealForms);
                Log.Information("CustomerSeal new output {@Output}", response);

            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal new error {@Error}", ex.Message); 
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
        public async Task<List<ResponseViewModel>> Update(CustomerSealUpdate customerSealUpdate)
        {
            List<ResponseViewModel> responses = new();
            try
            {
                Log.Information("CustomerSeal update input {@Input}", customerSealUpdate);
                responses = await customerSealService.Update(customerSealUpdate);
                Log.Information("CustomerSeal update output {@Output}", responses);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSeal update error {@Error}", ex.Message); 
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
                Log.Error("CustomerSeal pending error {@Error}", ex.Message); 
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
                Log.Error("CustomerSeal invalid error {@Error}", ex.Message); 
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
                Log.Error("CustomerSeal CancelReview error {@Error}", ex.Message); 
                response.DbError();
            }
            return response;
        }
    }
}

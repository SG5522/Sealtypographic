using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Models.TemplateConfig;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 用於排版管理的客戶印鑑搜尋管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TypographicCustomerSealController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerSealService customerSealService;
        private readonly ICustomerSealTemplateService customerSealTemplateService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicCustomerSealController(
                                                   ICustomerService customerService,
                                                   ICustomerSealService customerSealService,
                                                   ICustomerSealTemplateService customerSealTemplateService
                                              )
        {
            this.customerService = customerService;
            this.customerSealService = customerSealService;
            this.customerSealTemplateService = customerSealTemplateService;
        }

        /// <summary>
        /// 取得客戶資料列表(簡化資料的分頁)
        /// </summary>
        /// <param name="customerSearch">// 客戶搜尋</param>
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public CustomerPaginateShort Paginate([FromQuery] CustomerSearch customerSearch)
        {
            CustomerPaginateShort customerPaginateShort = new();
            try
            {
                Log.Information("TypographicSealSearch GetCustomerPaginate input {@Input}", customerSearch);
                customerPaginateShort = customerService.GetPaginateShort(customerSearch);
                Log.Information("TypographicCustomerSearch GetCustomerPaginate output {@Output}", customerPaginateShort);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicCustomerSearch GetCustomerPaginate error {@Error}", ex.Message);
                customerPaginateShort.DbError();
            }
            return customerPaginateShort;
        }

        /// <summary>
        /// 取得客戶印鑑季度
        /// </summary>
        /// <param name="customerId">客戶Id</param>        
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealQuarterResponse Quarter(int customerId)
        {
            CustomerSealQuarterResponse customerSealQuarterResponse = new ();
            try
            {
                Log.Information("TypographicCustomerSearch GetQuarter input {@Input}", customerId);
                customerSealQuarterResponse = customerSealService.GetQuarter(customerId);
                Log.Information("TypographicCustomerSearch GetQuarter output {@Output}", customerSealQuarterResponse);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicCustomerSearch GetQuarter error {@Error}", ex.Message);
                customerSealQuarterResponse.DbError();
            }
            return customerSealQuarterResponse;
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarterId"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealViewModels Seals(int customerSealQuarterId) 
        {
            CustomerSealViewModels customerSealViewModels = new();
            try
            {
                Log.Information("TypographicCustomerSearch seals input {@Input}", customerSealQuarterId);
                customerSealViewModels = customerSealService.GetSeals(customerSealQuarterId);
                Log.Information("TypographicCustomerSearch seals output {@Output}", customerSealViewModels);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicCustomerSearch seals error {@Error}", ex.Message);
                customerSealViewModels.DbError();
            }
            return customerSealViewModels;
        }

        /// <summary>
        /// 客戶印鑑樣板分頁列表
        /// </summary>
        /// <param name="paginateSearch">樣板分頁搜尋</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealTemplatePaginate TemplatePaginate([FromQuery] PaginateSearch paginateSearch)
        {
            CustomerSealTemplatePaginate customerSealTemplatePaginate = new();
            try
            {
                Log.Information("TypographicCustomerSearch templatePaginate input {@Input}", paginateSearch);
                customerSealTemplatePaginate = customerSealTemplateService.GetPaginateWithTypographic(paginateSearch);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicCustomerSearch templatePaginate error {@Error}", ex.Message);
                customerSealTemplatePaginate.DbError();
            }
            return customerSealTemplatePaginate;
        }

        /// <summary>
        /// 取得客戶印鑑樣板座標
        /// </summary>
        /// <param name="customerSealTemplateId">客戶印鑑樣本Id</param>        
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealTemplateDetailViewModel TemplateLocation(int customerSealTemplateId)
        {
            CustomerSealTemplateDetailViewModel customerSealTemplateDetailViewModel = new();
            try
            {
                Log.Information("TypographicCustomerSearch templateLocation input {@Input}", customerSealTemplateId);
                customerSealTemplateDetailViewModel = customerSealTemplateService.GetDetail(customerSealTemplateId);
                Log.Information("TypographicCustomerSearch templateLocation output {@Output}", customerSealTemplateDetailViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicCustomerSearch templateLocation error {@Error}", ex.Message);
                customerSealTemplateDetailViewModel.DbError();
            }
            return customerSealTemplateDetailViewModel;
        }
    }
}

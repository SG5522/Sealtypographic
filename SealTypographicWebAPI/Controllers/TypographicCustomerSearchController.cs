using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.TemplateConfig;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 用於排版管理的客戶搜尋API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TypographicCustomerSearchController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerSealService customerSealService;
        private readonly ILetterheadService letterheadService;
        private readonly ILetterheadImageService letterheadImageService;
        private readonly ITemporarySealService temporarySealService;        

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicCustomerSearchController(
                                                   ICustomerService customerService,
                                                   ICustomerSealService customerSealService,
                                                   ILetterheadService letterheadService,
                                                   ILetterheadImageService letterheadImageService,
                                                   ITemporarySealService temporarySealService
                                              )
        {
            this.customerService = customerService;
            this.customerSealService = customerSealService;
            this.letterheadService = letterheadService;
            this.letterheadImageService = letterheadImageService;
            this.temporarySealService = temporarySealService;
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
                Log.Error("TypographicCustomerSearch GetCustomerPaginate error {@Error}", ex);
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
                Log.Error("TypographicCustomerSearch GetQuarter error {@Error}", ex);
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
                Log.Error("TypographicCustomerSearch seals error {@Error}", ex);
                customerSealViewModels.DbError();
            }
            return customerSealViewModels;
        }
    }
}

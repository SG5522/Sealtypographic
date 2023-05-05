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
        //[HttpGet("[Action]/{id}")]
        [HttpGet("[Action]")]
        public CustomerPaginateShort GetCustomerPaginate([FromQuery] CustomerSearch customerSearch)
        {
            CustomerPaginateShort customerPaginateShort = new();
            try
            {
                Log.Information("TypographicSealSearch GetCustomerPaginate input {@Input}", customerSearch);
                customerPaginateShort = customerService.GetPaginateShort(customerSearch);
                Log.Information("TypographicSealSearch GetCustomerPaginate output {@Output}", customerPaginateShort);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicSealSearch GetCustomerPaginate error {@Error}", ex);
                customerPaginateShort.DbError();
            }
            return customerPaginateShort;
        }

        /// <summary>
        /// 排板分頁搜尋
        /// </summary>
        /// <param name="typographicPDFSearch">排版PDF關鍵字搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public TypographicPDFPaginateViewModel Paginate([FromQuery] TypographicPDFSearch typographicPDFSearch)
        {
            TypographicPDFPaginateViewModel typographicPDFPaginateViewModel = new ();
            return typographicPDFPaginateViewModel;
        }
    }
}

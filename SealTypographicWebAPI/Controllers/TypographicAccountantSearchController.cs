using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
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
    public class TypographicAccountantSearchController : ControllerBase
    {

        private readonly IAccountantService accountantService;
        private readonly IAccountantSignService accountantSignService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicAccountantSearchController(IAccountantService accountantService,IAccountantSignService accountantSignService)
        {
            this.accountantService = accountantService;
            this.accountantSignService = accountantSignService;
        }

        /// <summary>
        /// 取得客戶資料列表(簡化資料的分頁)
        /// </summary>
        /// <param name="accountantSearch">// 會計師搜尋</param>
        /// <returns></returns>
        //[HttpGet("[Action]/{id}")]
        [HttpGet("[Action]")]
        public AccountantPaginateViewModel GetCustomerPaginate([FromQuery] AccountantSearch accountantSearch)
        {
            AccountantPaginateViewModel accountantPaginateViewModel = new();
            try
            {
                Log.Information("TypographicSealSearch GetCustomerPaginate input {@Input}", accountantSearch);
                accountantPaginateViewModel = accountantService.GetPaginate(accountantSearch);
                Log.Information("TypographicSealSearch GetCustomerPaginate output {@Output}", accountantPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicSealSearch GetCustomerPaginate error {@Error}", ex);
                accountantPaginateViewModel.DbError();
            }
            return accountantPaginateViewModel;
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

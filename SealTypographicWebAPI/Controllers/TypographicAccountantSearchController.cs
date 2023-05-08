using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.AccountantGroup;
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
        private readonly IAccountantGroupService accountantGroupService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicAccountantSearchController(IAccountantService accountantService,IAccountantSignService accountantSignService, IAccountantGroupService accountantGroupService)
        {
            this.accountantService = accountantService;
            this.accountantSignService = accountantSignService;
            this.accountantGroupService = accountantGroupService;
        }

        /// <summary>
        /// 取得群組所有資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantGroupList GroupList()
        {
            AccountantGroupList accountantGroupList = new();
            try
            {
                accountantGroupList = accountantGroupService.GetAll();
                Log.Information("TypographicAccountantSearch GroupList output {@Output}", accountantGroupList);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups GroupList error {@Error}", ex.InnerException);
                accountantGroupList.DbError();
            }
            return accountantGroupList;
        }

        /// <summary>
        /// 取得會計師資料列表(簡化資料的分頁)
        /// </summary>
        /// <param name="accountantSearch">// 會計師搜尋</param>
        /// <returns></returns>
        //[HttpGet("[Action]/{id}")]
        [HttpGet("[Action]")]
        public AccountantPaginateViewModel Paginate([FromQuery] AccountantSearch accountantSearch)
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
                Log.Error("TypographicSealSearch GetCustomerPaginate error {@Error}", ex.InnerException);
                accountantPaginateViewModel.DbError();
            }
            return accountantPaginateViewModel;
        }
    }
}

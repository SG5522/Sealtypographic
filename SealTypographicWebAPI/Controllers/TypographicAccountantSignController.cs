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
    /// 用於排版管理的會計師簽印搜尋管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TypographicAccountantSignController : ControllerBase
    {

        private readonly IAccountantService accountantService;
        private readonly IAccountantSignService accountantSignService;
        private readonly IAccountantGroupService accountantGroupService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicAccountantSignController(IAccountantService accountantService,IAccountantSignService accountantSignService, IAccountantGroupService accountantGroupService)
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
                Log.Error("AccountantGroups GroupList error {@Error}", ex.Message);
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
                Log.Information("TypographicSealSearch paginate input {@Input}", accountantSearch);
                accountantPaginateViewModel = accountantService.GetPaginateWithTypographic(accountantSearch);
                Log.Information("TypographicSealSearch paginate output {@Output}", accountantPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicSealSearch paginate error {@Error}", ex.Message);
                accountantPaginateViewModel.DbError();
            }
            return accountantPaginateViewModel;
        }

        /// <summary>
        /// 取得會計師簽印(簡化資料的分頁)
        /// </summary>
        /// <param name="accountantSignGroupId"></param>        
        /// <returns></returns>
        //[HttpGet("[Action]/{id}")]
        [HttpGet("[Action]")]
        public AccountantSignViewModels Signs(int accountantSignGroupId)
        {
            AccountantSignViewModels accountantSignViewModels = new();
            try
            {
                Log.Information("TypographicSealSearch signs input {@Input}", accountantSignGroupId);
                accountantSignViewModels = accountantSignService.GetSignViewModels(accountantSignGroupId);
                Log.Information("TypographicSealSearch signs output {@Output}", accountantSignViewModels);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicSealSearch signs error {@Error}", ex.Message);
                accountantSignViewModels.DbError();
            }
            return accountantSignViewModels;
        }
    }
}

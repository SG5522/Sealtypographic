using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.BaseModels;
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
        private readonly IAccountantSignTemplateService accountantSignTemplateService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicAccountantSignController(IAccountantService accountantService,
                                                   IAccountantSignService accountantSignService, 
                                                   IAccountantGroupService accountantGroupService,
                                                   IAccountantSignTemplateService accountantSignTemplateService)
        {
            this.accountantService = accountantService;
            this.accountantSignService = accountantSignService;
            this.accountantGroupService = accountantGroupService;
            this.accountantSignTemplateService = accountantSignTemplateService;
        }

        /// <summary>
        /// 取得會計師群組所有資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantGroupList AccountantGroupList()
        {
            AccountantGroupList accountantGroupList = new();
            try
            {
                accountantGroupList = accountantGroupService.GetAll();
                Log.Information("TypographicAccountantSign accountantGroupList output {@Output}", accountantGroupList);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicAccountantSign accountantGroupList error {@Error}", ex.Message);
                accountantGroupList.DbError();
            }
            return accountantGroupList;
        }

        /// <summary>
        /// 取得會計師資料列表(簡化資料的分頁)
        /// </summary>
        /// <param name="accountantSearch">// 會計師搜尋</param>
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public AccountantPaginateViewModel Paginate([FromQuery] AccountantSearch accountantSearch)
        {
            AccountantPaginateViewModel accountantPaginateViewModel = new();
            try
            {
                Log.Information("TypographicAccountantSign paginate input {@Input}", accountantSearch);
                accountantPaginateViewModel = accountantService.GetPaginateWithTypographic(accountantSearch);
                Log.Information("TypographicAccountantSign paginate output {@Output}", accountantPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicAccountantSign paginate error {@Error}", ex.Message);
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
                Log.Information("TypographicAccountantSign signs input {@Input}", accountantSignGroupId);
                accountantSignViewModels = accountantSignService.GetSignViewModels(accountantSignGroupId);
                Log.Information("TypographicAccountantSign signs output {@Output}", accountantSignViewModels);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicAccountantSign signs error {@Error}", ex.Message);
                accountantSignViewModels.DbError();
            }
            return accountantSignViewModels;
        }

        /// <summary>
        /// 取得會計師簽印樣板分頁列表
        /// </summary>
        /// <param name="paginateSearch">樣板分頁搜尋</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantSignTemplatePaginate TemplatePaginate([FromQuery] PaginateSearch paginateSearch)
        {
            AccountantSignTemplatePaginate accountantSignTemplatePaginate = new();
            try
            {
                Log.Information("TypographicAccountantSign paginate input {@Input}", paginateSearch);
                accountantSignTemplatePaginate = accountantSignTemplateService.GetPaginateWithTypographic(paginateSearch);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicAccountantSign paginate error {@Error}", ex.Message);
                accountantSignTemplatePaginate.DbError();
            }
            return accountantSignTemplatePaginate;
        }

        /// <summary>                
        /// 取得會計師簽印樣板座標
        /// </summary>
        /// <param name="accountantSignTemplate"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantSignTemplateDetailViewModel TemplateLocation(int accountantSignTemplate)
        {
            AccountantSignTemplateDetailViewModel accountantSignTemplateDetailViewModel = new();
            try
            {
                Log.Information("TypographicAccountantSign templateLocation input {@Input}", accountantSignTemplate);
                accountantSignTemplateDetailViewModel = accountantSignTemplateService.GetDetail(accountantSignTemplate);
                Log.Information("TypographicAccountantSign templateLocation output {@Output}", accountantSignTemplateDetailViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicAccountantSign templateLocation error {@Error}", ex.Message);
                accountantSignTemplateDetailViewModel.DbError();
            }
            return accountantSignTemplateDetailViewModel;
        }
    }
}

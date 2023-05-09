using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.TemplateConfig;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 用於排版管理的臨時章搜尋管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TypographicTemporarySealController : ControllerBase
    {
        private readonly ITemporarySealService temporarySealService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicTemporarySealController(ITemporarySealService temporarySealService)
        {
            this.temporarySealService = temporarySealService;
        }

        /// <summary>
        /// 取得信頭資料列表(分頁)
        /// </summary>
        /// <param name="temporarySealSearch">信頭分頁搜尋</param>
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public TemporarySealPaginateViewModel Paginate([FromQuery] TemporarySealSearch temporarySealSearch)
        {
            TemporarySealPaginateViewModel temporarySealPaginateViewModel = new();
            try
            {
                Log.Information("TypographicTemporarySeal paginate input {@Input}", temporarySealSearch);
                temporarySealPaginateViewModel = temporarySealService.GetPaginate(temporarySealSearch);
                Log.Information("TypographicTemporarySeal paginate output {@Output}", temporarySealPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicTemporarySeal paginate error {@Error}", ex.Message);
                temporarySealPaginateViewModel.DbError();
            }
            return temporarySealPaginateViewModel;
        }
        
    }
}

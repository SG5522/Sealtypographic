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
        /// 取得臨時章資料列表(分頁)
        /// </summary>
        /// <param name="typographicTemporarySealSearch">臨時章分頁搜尋</param>
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public TypographicTemporarySealPaginateViewModel Paginate([FromQuery] TypographicTemporarySealSearch typographicTemporarySealSearch)
        {
            TypographicTemporarySealPaginateViewModel typographicTemporarySealPaginateViewModel = new();
            try
            {
                Log.Information("TypographicTemporarySeal paginate input {@Input}", typographicTemporarySealSearch);
                typographicTemporarySealPaginateViewModel = temporarySealService.GetPaginateWithTypographic(typographicTemporarySealSearch);
                Log.Information("TypographicTemporarySeal paginate output {@Output}", typographicTemporarySealPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicTemporarySeal paginate error {@Error}", ex.Message);
                typographicTemporarySealPaginateViewModel.DbError();
            }
            return typographicTemporarySealPaginateViewModel;
        }

        /// <summary>
        /// 取得臨時章
        /// </summary>
        /// <param name="temporaryId">臨時章Id</param>
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public TemporarySealDetailViewModel Seals(int temporaryId)
        {
            TemporarySealDetailViewModel typographicTemporarySealPaginateViewModel = new();
            try
            {
                Log.Information("TypographicTemporarySeal seals input {@Input}", temporaryId);
                typographicTemporarySealPaginateViewModel = temporarySealService.GetDetail(temporaryId);
                Log.Information("TypographicTemporarySeal seals output {@Output}", typographicTemporarySealPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicTemporarySeal seals error {@Error}", ex.Message);
                typographicTemporarySealPaginateViewModel.DbError();
            }
            return typographicTemporarySealPaginateViewModel;
        }

    }
}

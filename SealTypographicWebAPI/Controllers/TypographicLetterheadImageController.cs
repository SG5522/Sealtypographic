using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.TemplateConfig;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 用於排版管理的信頭圖片搜尋管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TypographicLetterheadImageController : ControllerBase
    {
        private readonly ILetterheadService letterheadService;
        private readonly ILetterheadImageService letterheadImageService;           

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicLetterheadImageController(ILetterheadService letterheadService, ILetterheadImageService letterheadImageService)
        {
            this.letterheadService = letterheadService;
            this.letterheadImageService = letterheadImageService;
        }

        /// <summary>
        /// 取得信頭資料列表(分頁)
        /// </summary>
        /// <param name="letterheadSearch">信頭分頁搜尋</param>
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public LetterheadPaginateViewModel Paginate([FromQuery] LetterheadSearch letterheadSearch)
        {
            LetterheadPaginateViewModel letterheadPaginateViewModel = new();
            try
            {
                Log.Information("TypographicLetterheadSearch paginate input {@Input}", letterheadSearch);
                letterheadPaginateViewModel = letterheadService.GetPaginate(letterheadSearch);
                Log.Information("TypographicLetterheadSearch paginate output {@Output}", letterheadPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicLetterheadSearch paginate error {@Error}", ex.Message);
                letterheadPaginateViewModel.DbError();
            }
            return letterheadPaginateViewModel;
        }

        /// <summary>
        /// 取得信頭圖片
        /// </summary>
        /// <param name="letterheadImageId">信頭圖片Id</param>
        /// <returns></returns>
        [HttpGet]
        public LetterheadImageViewModel ImageViewModel(int letterheadImageId)
        {
            LetterheadImageViewModel letterheadImageViewModel = new();
            try
            {
                Log.Information("TypographicLetterheadSearch imageViewModel input {@Input}", letterheadImageId);
                letterheadImageViewModel = letterheadImageService.GetImageViewModel(letterheadImageId);
                Log.Information("TypographicLetterheadSearch imageViewModel output {@Output}", letterheadImageViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicLetterheadSearch imageViewModel error {@Error}", ex.Message);
                letterheadImageViewModel.DbError();
            }
            return letterheadImageViewModel;
        }
    }
}

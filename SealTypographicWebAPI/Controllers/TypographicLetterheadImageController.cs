using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.LetterheadTemplate;
using SealTypographicWebAPI.Services;
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
        private readonly ILetterheadImageTemplateService letterheadImageTemplateService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicLetterheadImageController(ILetterheadService letterheadService, ILetterheadImageService letterheadImageService, ILetterheadImageTemplateService letterheadImageTemplateService)
        {
            this.letterheadService = letterheadService;
            this.letterheadImageService = letterheadImageService;
            this.letterheadImageTemplateService = letterheadImageTemplateService;
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
                Log.Information("TypographicLetterheadImage paginate input {@Input}", letterheadSearch);
                letterheadPaginateViewModel = letterheadService.GetPaginate(letterheadSearch);
                Log.Information("TypographicLetterheadImage paginate output {@Output}", letterheadPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicLetterheadImage paginate error {@Error}", ex.Message);
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
                Log.Information("TypographicLetterheadImage imageViewModel input {@Input}", letterheadImageId);
                letterheadImageViewModel = letterheadImageService.GetImageViewModel(letterheadImageId);
                Log.Information("TypographicLetterheadImage imageViewModel output {@Output}", letterheadImageViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicLetterheadImage imageViewModel error {@Error}", ex.Message);
                letterheadImageViewModel.DbError();
            }
            return letterheadImageViewModel;
        }

        /// <summary>
        /// 信頭樣板分頁列表
        /// </summary>
        /// <param name="paginateSearch">信頭樣板分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public LetterheadImageTemplatePaginate TemplatePaginate([FromQuery] PaginateSearch paginateSearch)
        {
            LetterheadImageTemplatePaginate letterheadTemplatePaginate = new();
            try
            {
                Log.Information("LetterheadTemplate paginate input {@Input}", paginateSearch);
                letterheadTemplatePaginate = letterheadImageTemplateService.GetPaginateWithTypographic(paginateSearch);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadTemplate paginate error {@Error}", ex.Message);
                letterheadTemplatePaginate.DbError();
            }
            return letterheadTemplatePaginate;
        }

        /// <summary>        
        /// 取得信頭樣板座標
        /// </summary>
        /// <param name="letterheadImageTemplateId"></param>
        /// <returns></returns>
        [HttpGet("{letterheadImageTemplateId}")]
        public LetterheadImageTemplateDetailViewModel TemplateLocation(int letterheadImageTemplateId)
        {
            LetterheadImageTemplateDetailViewModel letterheadTemplateDetailViewModel = new();
            try
            {
                Log.Information("TypographicLetterheadImage templateLocation input {@Input}", letterheadImageTemplateId);
                letterheadTemplateDetailViewModel = letterheadImageTemplateService.GetDetail(letterheadImageTemplateId);
                Log.Information("TypographicLetterheadImage templateLocation output {@Output}", letterheadTemplateDetailViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicLetterheadImage templateLocation error {@Error}", ex.Message);
                letterheadTemplateDetailViewModel.DbError();
            }
            return letterheadTemplateDetailViewModel;
        }
    }
}

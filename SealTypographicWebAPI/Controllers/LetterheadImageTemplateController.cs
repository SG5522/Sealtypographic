using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.LetterheadTemplate;
using SealTypographicWebAPI.Services;
using Serilog;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 信頭樣板管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LetterheadImageTemplateController : ControllerBase
    {
        private readonly ILetterheadImageTemplateService letterheadImageTemplateService;

        /// <summary>
        /// 建構 注入Service
        /// </summary>        
        public LetterheadImageTemplateController(ILetterheadImageTemplateService letterheadImageTemplateService)
        {
            this.letterheadImageTemplateService = letterheadImageTemplateService;
        }

        /// <summary>
        /// 信頭樣板詳細
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public LetterheadImageTemplateDetailViewModel Detail(int id)
        {
            LetterheadImageTemplateDetailViewModel letterheadTemplateDetailViewModel = new();
            try
            {
                Log.Information("LetterheadTemplate detail input {@Input}", id);
                letterheadTemplateDetailViewModel = letterheadImageTemplateService.GetDetail(id);
                Log.Information("LetterheadTemplate detail output {@Output}", letterheadTemplateDetailViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadTemplate detail error {@Error}", ex.Message); 
                letterheadTemplateDetailViewModel.DbError();
            }
            return letterheadTemplateDetailViewModel;
        }

        /// <summary>
        /// 信頭簽印樣板圖片顯示
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]/{id}")]
        public LetterheadImageTemplateImageView ViewImage(int id)
        {
            LetterheadImageTemplateImageView viewImage = new();
            try
            {
                Log.Information("LetterheadImageTemplate viewImage input {@Input}", id);
                viewImage = letterheadImageTemplateService.GetImage(id);
                Log.Information("LetterheadImageTemplate viewImage output {@Output}", id);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImageTemplate viewImage error {@Error}", ex.Message); 
                viewImage.DbError();
            }
            return viewImage;
        }

        /// <summary>
        /// 信頭樣板分頁列表
        /// </summary>
        /// <param name="letterheadTemplateSearch">信頭樣板分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public LetterheadImageTemplatePaginate Paginate([FromQuery] LetterheadImageTemplateSearch letterheadTemplateSearch)
        {
            LetterheadImageTemplatePaginate letterheadTemplatePaginate = new ();
            try
            {
                Log.Information("LetterheadTemplate paginate input {@Input}", letterheadTemplateSearch);
                letterheadTemplatePaginate = letterheadImageTemplateService.GetPaginate(letterheadTemplateSearch);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadTemplate paginate error {@Error}", ex.Message); 
                letterheadTemplatePaginate.DbError();
            }
            return letterheadTemplatePaginate;
        }

        /// <summary>
        /// 新增信頭樣板
        /// </summary>
        /// <param name="letterheadTemplateForm"></param>        
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(LetterheadImageTemplateForm letterheadTemplateForm)
        {
            ResponseViewModel response = new ();
            try
            {                
                Log.Information("LetterheadTemplate new input {@Input}", letterheadTemplateForm);        
                response = await letterheadImageTemplateService.New(letterheadTemplateForm);
                Log.Information("LetterheadTemplate new output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadTemplate new error {@Error}", ex.Message); 
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 更新信頭樣板
        /// </summary>
        /// <param name="letterheadTemplateUpdateForm">信頭樣板</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseViewModel> Update(LetterheadImageTemplateUpdateForm letterheadTemplateUpdateForm)
        {
            ResponseViewModel response = new();
            try
            {                
                Log.Information("LetterheadTemplate update input {@input}", letterheadTemplateUpdateForm);
                response = await letterheadImageTemplateService.Update(letterheadTemplateUpdateForm);
                Log.Information("LetterheadTemplate update output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadTemplate update error {@Error}", ex.Message); 
                response.DbError();
            }

            return response;
        }

        /// <summary>
        /// 刪除信頭樣板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ResponseViewModel Delete(int id)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("LetterheadTemplate delete input  {@id}", id);
                response = letterheadImageTemplateService.Delete(id);
                Log.Information("LetterheadTemplate delete output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadTemplate delete error {@Error}", ex.Message); 
                response.DbError();
            }

            return response;
        }
    }
}

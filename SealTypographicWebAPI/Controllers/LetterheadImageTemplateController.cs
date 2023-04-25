using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.LetterheadTemplate;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
                Log.Error("LetterheadTemplate detail error {@Error}", ex);
                letterheadTemplateDetailViewModel.DbError();
            }
            return letterheadTemplateDetailViewModel;
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
                Log.Error("LetterheadTemplate paginate error {@Error}", ex);
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
        public async Task<ResponseViewModel> New([FromForm]LetterheadImageTemplateForm letterheadTemplateForm)
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
                Log.Error("LetterheadTemplate new error {@Error}", ex);
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
        public async Task<ResponseViewModel> Update([FromForm] LetterheadImageTemplateUpdateForm letterheadTemplateUpdateForm)
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
                Log.Error("LetterheadTemplate update error {@Error}", ex);
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
                Log.Error("LetterheadTemplate delete error {@Error}", ex);
                response.DbError();
            }

            return response;
        }
    }
}

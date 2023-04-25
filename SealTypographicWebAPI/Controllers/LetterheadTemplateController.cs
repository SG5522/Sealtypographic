using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.LetterheadTemplate;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 信頭樣板管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LetterheadTemplateController : ControllerBase
    {
        

        /// <summary>
        /// 建構 注入Service
        /// </summary>
        
        public LetterheadTemplateController()
        {
            
        }

        /// <summary>
        /// 信頭樣板詳細
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public LetterheadTemplateDetailViewModel Detail(int id)
        {
            LetterheadTemplateDetailViewModel letterheadTemplateDetailViewModel = new();
            try
            {
                Log.Information("LetterheadTemplate detail input {@Input}", id);
                
                Log.Information("LetterheadTemplate detail output {@Output}", id);
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
        public LetterheadTemplatePaginate Paginate([FromQuery] LetterheadTemplateSearch letterheadTemplateSearch)
        {
            LetterheadTemplatePaginate letterheadTemplatePaginate = new ();
            try
            {
                Log.Information("LetterheadTemplate paginate input {@Input}", letterheadTemplateSearch);
                         
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
        public async Task<ResponseViewModel> New([FromForm]LetterheadTemplateForm letterheadTemplateForm)
        {
            ResponseViewModel response = new ();
            try
            {                
                Log.Information("LetterheadTemplate new input {@Input}", letterheadTemplateForm);                
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
        public async Task<ResponseViewModel> Update([FromForm] LetterheadTemplateUpdateForm letterheadTemplateUpdateForm)
        {
            ResponseViewModel response = new();
            try
            {                
                Log.Information("LetterheadTemplate update input {@input}", letterheadTemplateUpdateForm);
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

using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Util;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 信頭資料管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LetterheadController : ControllerBase
    {
        /// <summary>
        /// 宣告會計師資料處理的interface
        /// </summary>
        protected readonly ILetterheadService letterheadService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="letterheadService"></param>
        public LetterheadController(ILetterheadService letterheadService)
        {
            this.letterheadService = letterheadService;
        }

        /// <summary>
        /// 取得信頭資料列表
        /// </summary>
        /// <param name="letterheadSearch"></param>
        /// <returns></returns>
        [HttpGet]
        public LetterheadViewModels Get([FromQuery] LetterheadSearch letterheadSearch)
        {
            try
            {
                Log.Information("Letterhead get LetterheadViewModels input {@Input}", letterheadSearch);
                LetterheadViewModels letterheadResponse = letterheadService.GetLetterheadViewModels(letterheadSearch);
                Log.Information("Letterhead get LetterheadViewModels output {@Output}", letterheadResponse);
                return letterheadResponse;
            }
            catch (Exception ex)
            {
                Log.Error("Letterhead get LetterheadViewModels error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
                return new()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 取得信頭基本資料
        /// </summary>
        /// <param name="letterheadId">信頭Id</param>
        /// <returns></returns>
        [HttpGet("{letterheadId}")]
        public LetterheadResponse Get(int letterheadId)
        {
            try
            {
                Log.Information("Letterhead get LetterheadViewModel input {@Input}", letterheadId);
                LetterheadResponse letterheadResponse = letterheadService.GetLetterheadViewModel(letterheadId);
                Log.Information("Letterhead get LetterheadViewModel output {@Output}", letterheadResponse);
                return letterheadResponse;
            }
            catch (Exception ex)
            {
                Log.Error("Letterhead get LetterheadViewModel error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
                return new ()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 建立信頭
        /// </summary>
        /// <param name="letterheadPostData"></param>
        [HttpPost]
        public ResponseViewModel Post([FromBody] LetterheadForm letterheadPostData)
        {
            try
            {
                Log.Information("Letterhead post letterheadPostData input {@Input}", letterheadPostData);
                ResponseViewModel response = letterheadService.CreateLetterhead(letterheadPostData);
                Log.Information("Letterhead post letterheadPostData output {@Output}", response);
                return response;
            }
            catch(Exception ex) 
            {
                Log.Error("Letterhead post letterheadPostData error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }

        /// <summary>
        /// 修改信頭
        /// </summary>
        /// <param name="letterheadPostData"></param>
        [HttpPut]
        public ResponseViewModel Put([FromBody] LetterheadFormUpdate letterheadPostData)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("Letterhead put letterheadPostData input {@Input}", letterheadPostData);
                response = letterheadService.UpdateLetterhead(letterheadPostData);
                Log.Information("Letterhead put letterheadPostData output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Letterhead put letterheadPostData error {@Error}", ex);
                response.DbError();
                return response;
            }
        }

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="letterheadId"></param>
        /// <returns></returns>
        [HttpDelete("{letterheadId}")]
        public ResponseViewModel Delete(int letterheadId)
        {
            try
            {
                Log.Information("Letterhead delete(hide) letterheadPostData input {@Input}", letterheadId);
                ResponseViewModel response = letterheadService.DeleteLetterhead(letterheadId);
                Log.Information("Letterhead delete(hide) letterheadPostData output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Letterhead delete(hide) letterheadPostData error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }
    }
}

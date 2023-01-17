using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Services;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 信頭資料管理
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
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
        public LetterheadPaginateViewModel Get([FromQuery] LetterheadSearch letterheadSearch)
        {
            LetterheadPaginateViewModel letterheadPaginateViewModel = new();
            try
            {
                Log.Information("Letterhead get input {@Input}", letterheadSearch);
                letterheadPaginateViewModel = letterheadService.GetPaginate(letterheadSearch);
                Log.Information("Letterhead get output {@Output}", letterheadPaginateViewModel);                
            }
            catch (Exception ex)
            {
                Log.Error("Letterhead get error {@Error}", ex);
                letterheadPaginateViewModel.DbError();                
            }
            return letterheadPaginateViewModel;
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
            ResponseViewModel responseViewModel = new();
            try
            {
                Log.Information("Letterhead delete(hide) input {@Input}", letterheadId);
                responseViewModel = letterheadService.Delete(letterheadId);
                Log.Information("Letterhead delete(hide) output {@Output}", responseViewModel);                
            }
            catch (Exception ex)
            {
                Log.Error("Letterhead delete(hide) error {@Error}", ex);
                responseViewModel.DbError();                
            }
            return responseViewModel;
        }
    }
}

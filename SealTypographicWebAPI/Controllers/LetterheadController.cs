using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Services;
using Serilog;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理信頭資料
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    public class LetterheadController : ControllerBase
    {
        /// <summary>
        /// 管理信頭資料的service
        /// </summary>
        private readonly ILetterheadService letterheadService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="letterheadService">管理信頭資料的service</param>
        public LetterheadController(ILetterheadService letterheadService)
        {
            this.letterheadService = letterheadService;
        }

        /// <summary>
        /// 取得信頭資料列表(分頁)
        /// </summary>
        /// <param name="letterheadSearch">信頭分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public LetterheadPaginateViewModel Paginate([FromQuery] LetterheadSearch letterheadSearch)
        {
            LetterheadPaginateViewModel letterheadPaginateViewModel = new();
            try
            {
                Log.Information("LetterheadImage get paginate input {@Input}", letterheadSearch);
                letterheadPaginateViewModel = letterheadService.GetPaginate(letterheadSearch);
                Log.Information("LetterheadImage get paginate output {@Output}", letterheadPaginateViewModel);                
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage get paginate error {@Error}", ex.Message);
                letterheadPaginateViewModel.DbError();                
            }
            return letterheadPaginateViewModel;
        }

        /// <summary>       
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。        
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ResponseViewModel Delete(int id)
        {
            ResponseViewModel responseViewModel = new();
            try
            {
                Log.Information("LetterheadImage delete(hide) input {@Input}", id);
                responseViewModel = letterheadService.Delete(id);
                Log.Information("LetterheadImage delete(hide) output {@Output}", responseViewModel);                
            }
            catch (Exception ex)
            {
                Log.Error("LetterheadImage delete(hide) error {@Error}", ex.Message);
                responseViewModel.DbError();                
            }
            return responseViewModel;
        }
    }
}

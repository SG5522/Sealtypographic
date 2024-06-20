using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Services;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理信頭資料
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    [Authorize(Roles = KeycloakRoleConsts.DATAMANAGE_LETTERHEAD)]
    public class LetterheadController : APIControllerBase
    {
        /// <summary>
        /// 管理信頭資料的service
        /// </summary>
        private readonly ILetterheadService letterheadService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="letterheadService">管理信頭資料的service</param>
        /// <param name="applicationUserService"></param>
        public LetterheadController(ILetterheadService letterheadService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.letterheadService = letterheadService;
        }

        /// <summary>
        /// 取得信頭資料列表(分頁)
        /// </summary>
        /// <param name="letterheadSearch">信頭分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LetterheadPaginateViewModel> Paginate([FromQuery] LetterheadSearch letterheadSearch) => await letterheadService.GetPaginate(letterheadSearch);

        /// <summary>       
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。        
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResponseViewModel> Delete(int id) => await letterheadService.Delete(id);
    }
}

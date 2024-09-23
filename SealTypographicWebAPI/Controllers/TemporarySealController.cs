using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Services;
using Serilog;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 臨時章管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = KeycloakRoleConsts.DATAMANAGE_TEMPORARYSEAL)]
    public class TemporarySealController : APIControllerBase
    {
        /// <summary>
        /// 管理臨時章資料
        /// </summary>
        private readonly ITemporarySealService temporarySealService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TemporarySealController(ITemporarySealService temporarySealService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.temporarySealService = temporarySealService;
        }

        /// <summary>
        /// 取得臨時章列表
        /// </summary>
        /// <param name="temporarySealSearch">臨時章分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TemporarySealPaginateViewModel> Paginate([FromQuery] TemporarySealSearch temporarySealSearch) 
            => temporarySealService.GetPaginate(temporarySealSearch, await GetUserId());

        /// <summary>
        /// 取得臨時章印鑑組
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isTransparent"></param>
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public async Task<TemporarySealDetailViewModel> Detail([FromQuery] int id, bool isTransparent) 
            => temporarySealService.GetDetail(id, await GetUserId(), isTransparent);

        /// <summary>
        /// 新增臨時章
        /// </summary>
        /// <param name="temporarySealForm">臨時章資料</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(TemporarySealForm temporarySealForm) 
            => await temporarySealService.New(temporarySealForm, await GetUserId());

        /// <summary>
        /// 更新臨時章
        /// </summary>        
        /// <param name="temporarySealUpdateForm">異動臨時章</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseViewModel> Update(TemporarySealUpdateForm temporarySealUpdateForm) 
            => await temporarySealService.Update(temporarySealUpdateForm, await GetUserId());


        /// <summary>
        /// 刪除臨時章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResponseViewModel> Delete(int id) => temporarySealService.Delete(id, await GetUserId());
    }
}

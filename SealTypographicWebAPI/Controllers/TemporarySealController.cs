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
        public TemporarySealPaginateViewModel Paginate([FromQuery] TemporarySealSearch temporarySealSearch)
        {
            TemporarySealPaginateViewModel temporaryPaginateViewModel = new();            
            try
            {
                Log.Information("TemporarySeal paginate input {@Input}", temporarySealSearch);
                temporaryPaginateViewModel = temporarySealService.GetPaginate(temporarySealSearch);
                Log.Information("TemporarySeal paginate output {@Output}", temporaryPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TemporarySeal get paginate error {@Error}", ex.Message); 
                temporaryPaginateViewModel.DbError();
            }
            return temporaryPaginateViewModel;
        }

        /// <summary>
        /// 取得臨時章印鑑組
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isTransparent"></param>
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public TemporarySealDetailViewModel Detail([FromQuery] int id, bool isTransparent)
        {
            TemporarySealDetailViewModel temporarySealDetailViewModel = new();
            try
            {
                Log.Information("TemporarySeal detail input {@Input}", id);
                temporarySealDetailViewModel = temporarySealService.GetDetail(id, isTransparent);               
            }
            catch (Exception ex)
            {
                Log.Error("TemporarySeal get detail error {@Error}", ex.Message); 
                temporarySealDetailViewModel.DbError();
            }
            return temporarySealDetailViewModel;
        }

        /// <summary>
        /// 新增臨時章
        /// </summary>
        /// <param name="temporarySealForm">臨時章資料</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(TemporarySealForm temporarySealForm)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("TemporarySeal new input {@Input}", temporarySealForm);
                response = await temporarySealService.New(temporarySealForm, await GetUserId());
                Log.Information("TemporarySeal new output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("TemporarySeal new error {@Error}", ex.Message); 
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 更新臨時章
        /// </summary>        
        /// <param name="temporarySealUpdateForm">異動臨時章</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseViewModel> Update(TemporarySealUpdateForm temporarySealUpdateForm)
        {
            ResponseViewModel response = new();
            try
            {                
                Log.Information("TemporarySeal update input temporarySealUpdateForm {@temporarySealUpdateForm}", temporarySealUpdateForm);
                response = await temporarySealService.Update(temporarySealUpdateForm, await GetUserId());
                Log.Information("TemporarySeal update output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("TemporarySeal new update error {@Error}", ex.Message); 
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 刪除臨時章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ResponseViewModel Delete(int id)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("TemporarySeal delete input {@Input}", response);
                response = temporarySealService.Delete(id);
                Log.Information("TemporarySeal delete output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("TemporarySeal delete error {@Error}", ex.Message); 
                response.DbError();
            }
            return response;
        }
    }
}

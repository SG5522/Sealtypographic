using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 取得臨時章
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TemporarySealController : ControllerBase
    {
        /// <summary>
        /// 管理臨時章資料
        /// </summary>
        private readonly ITemporarySealService temporarySealService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TemporarySealController(ITemporarySealService temporaryService) 
        {
            this.temporarySealService = temporaryService;
        }

        /// <summary>
        /// 取得臨時章列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public TemporarySealPaginateViewModel Paginate([FromQuery]TemporarySealSearch temporarySealSearch)
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
                Log.Error("TemporarySeal get paginate error {@Error}", ex);
                temporaryPaginateViewModel.DbError();
            }
            return temporaryPaginateViewModel;
        }

        /// <summary>
        /// 取得臨時章印鑑組
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        [HttpGet("{id}")]
        public TemporarySealDetailViewModel Detail(int id)
        {
            TemporarySealDetailViewModel temporarySealDetailViewModel = new();
            try
            {
                Log.Information("TemporarySeal detail input {@Input}", id);
                temporarySealDetailViewModel = temporarySealService.GetDetail(id);
                //Log.Information("TemporarySeal detail output {@Output}", temporarySealDetailViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TemporarySeal get detail error {@Error}", ex);
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
        public ResponseViewModel New(TemporarySealForm temporarySealForm)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("TemporarySeal new input {@Input}", temporarySealForm);
                response = temporarySealService.New(temporarySealForm);
                Log.Information("TemporarySeal new output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("TemporarySeal new error {@Error}", ex);
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 更新臨時章
        /// </summary>
        /// <param name="id">臨時章id</param>
        /// <param name="temporarySealUpdateForm"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ResponseViewModel Update(int id, TemporarySealUpdateForm temporarySealUpdateForm)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("TemporarySeal update input {@Input}", temporarySealUpdateForm);
                response = temporarySealService.Update(id, temporarySealUpdateForm);
                Log.Information("TemporarySeal update output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("TemporarySeal new update error {@Error}", ex);
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
                Log.Error("TemporarySeal delete error {@Error}", ex);
                response.DbError();
            }
            return response;
        }
    }
}

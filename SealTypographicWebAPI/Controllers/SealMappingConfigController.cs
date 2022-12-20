using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Models.SealMappingConfig;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理圖片群組
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SealMappingConfigController : ControllerBase
    {
        /// <summary>
        /// 宣告會計師資料處理的interface
        /// </summary>
        protected readonly SealMappingConfigService sealMappingConfigService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="imageGroupService">圖片群組</param>        
        public SealMappingConfigController(SealMappingConfigService imageGroupService)
        {
            this.sealMappingConfigService = imageGroupService;            
        }

        /// <summary>
        /// 取得印鑑類別資料列表
        /// </summary>
        /// <param name="sealType">印鑑類別 1.客戶 2.會計師 3.信頭</param>        
        /// <returns></returns>
        [HttpGet]
        public SealMappingConfigResponseList Get(SealType sealType)
        {
            try
            {
                Log.Information("CustomerSeal get(sealtype) input {@Input}", sealType);
                SealMappingConfigResponseList sealMappingConfigResponseList = sealMappingConfigService.GetSealMappingConfigResponseList(sealType);
                Log.Information("CustomerSeal get(sealtype) output {@Output}", sealMappingConfigResponseList);
                return sealMappingConfigResponseList;
            }
            catch (Exception ex)
            {
                Log.Error("sealMappingConfigViewModel get{id} error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
                return new SealMappingConfigResponseList()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 取得圖片群組資料
        /// </summary>
        /// <param name="id">群組ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public SealMappingConfigResponse Get(int id)
        {
            try
            {
                Log.Information("CustomerSeal get{id} input {@Input}", id);
                SealMappingConfigResponse sealMappingConfigResponse = sealMappingConfigService.GetImageGroup(id); ;
                Log.Information("CustomerSeal get{id} output {@Output}", sealMappingConfigResponse);
                return sealMappingConfigResponse;
            }
            catch (Exception ex)
            {
                Log.Error("sealMappingConfigViewModel get{id} error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
                return new ()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 建立圖片群組
        /// </summary>
        /// <param name="sealMappingConfigViewModel">群組資料</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseViewModel Post(SealMappingConfigViewModel sealMappingConfigViewModel)
        {
            try
            {
                Log.Information("CustomerSeal post input {@Input}", sealMappingConfigViewModel);
                ResponseViewModel response = sealMappingConfigService.CreateSealMappingConfig(sealMappingConfigViewModel);
                Log.Information("CustomerSeal post output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("sealMappingConfigViewModel post error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="sealMappingConfigViewModel">群組資料</param>       
        [HttpPut]
        public ResponseViewModel Put(SealMappingConfigViewModel sealMappingConfigViewModel)
        {
            try
            {
                Log.Information("CustomerSeal post input {@Input}", sealMappingConfigViewModel);
                ResponseViewModel response = sealMappingConfigService.UpdateSealMappingConfig(sealMappingConfigViewModel);
                Log.Information("CustomerSeal post output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("sealMappingConfigViewModel put error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }
    }
}

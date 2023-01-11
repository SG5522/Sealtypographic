using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Models.SealMappingConfig;
using Serilog;
using SealTypographicWebAPI.Services.Implements;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 取得印鑑類型列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SealMappingConfigController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly SealMappingConfigService sealMappingConfigService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="sealMappingConfigService"></param>        
        public SealMappingConfigController(SealMappingConfigService sealMappingConfigService)
        {
            this.sealMappingConfigService = sealMappingConfigService;            
        }

        /// <summary>
        /// 取得印鑑類型列表
        /// </summary>
        /// <param name="sealType">印鑑類別 1.客戶 2.會計師 </param>        
        /// <returns></returns>
        [HttpGet]
        public SealMappingConfigResponseList Get(SealType sealType)
        {
            SealMappingConfigResponseList sealMappingConfigResponseList = new ();
            try
            {
                Log.Information("SealMappingConfig get input {@Input}", sealType);
                sealMappingConfigResponseList = sealMappingConfigService.Get(sealType);
                Log.Information("SealMappingConfig get output {@Output}", sealMappingConfigResponseList);                
            }
            catch (Exception ex)
            {
                Log.Error("SealMappingConfig get error {@Error}", ex);
                sealMappingConfigResponseList.DbError();
            }
            return sealMappingConfigResponseList;
        }          
    }
}

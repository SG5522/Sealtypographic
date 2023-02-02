using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
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
        /// 取得印鑑類型列表的service
        /// </summary>
        private readonly SealMappingConfigService sealMappingConfigService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="sealMappingConfigService">取得印鑑類型列表的service</param>        
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
        public SealMappingConfigResponseList ConfigList(SealType sealType)
        {
            SealMappingConfigResponseList sealMappingConfigResponseList = new ();
            try
            {
                Log.Information("SealMappingConfig ConfigList input {@Input}", sealType);
                sealMappingConfigResponseList = sealMappingConfigService.Get(sealType);
                Log.Information("SealMappingConfig ConfigList output {@Output}", sealMappingConfigResponseList);                
            }
            catch (Exception ex)
            {
                Log.Error("SealMappingConfig ConfigList error {@Error}", ex);
                sealMappingConfigResponseList.DbError();
            }
            return sealMappingConfigResponseList;
        }          
    }
}

using Microsoft.AspNetCore.Mvc;
using Serilog;
using SealTypographicWebAPI.Services.Implements;
using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 取得印鑑類型列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseCodeController : ControllerBase
    {
        /// <summary>
        /// 取得回應代碼的service
        /// </summary>
        private readonly ResponseCodeService responseCodeService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="responseCodeService">取得回應代碼的service</param>        
        public ResponseCodeController(ResponseCodeService responseCodeService)
        {
            this.responseCodeService = responseCodeService;
        }


        /// <summary>
        /// 取得回應代碼列表
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        public ResponseCodeList ResponseCodeList()
        {
            ResponseCodeList responseCodeList = new ();
            try
            {                
                responseCodeList = responseCodeService.Get();
                Log.Information("SealMappingConfig ConfigList output {@Output}", responseCodeList);                
            }
            catch (Exception ex)
            {
                Log.Error("SealMappingConfig ConfigList error {@Error}", ex);                
            }
            return responseCodeList;
        }          
    }
}

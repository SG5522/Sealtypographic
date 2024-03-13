using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.ReviewStatusList;
using SealTypographicWebAPI.Models.TemplateConfig;
using SealTypographicWebAPI.Services.Implements;
using Serilog;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 取得樣板各項目參數
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateConfigController : ControllerBase
    {
        /// <summary>
        /// 取得樣板各項目列表的service
        /// </summary>
        private readonly TemplateConfigService templateConfigService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="templateConfigService">取得印鑑類型列表的service</param>        
        public TemplateConfigController(TemplateConfigService templateConfigService)
        {
            this.templateConfigService = templateConfigService;
        }

        /// <summary>
        /// 取得頁面方向
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TemplateConfigResponseList PapeOrientation() => templateConfigService.GetEnumData(typeof(PapeOrientation));

        /// <summary>
        /// 取得頁面格式
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TemplateConfigResponseList PageSize() => templateConfigService.GetEnumData(typeof(PageSize));

        /// <summary>
        /// 取得樣板(客戶)疊放方式
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TemplateConfigResponseList StackMode()
        {
            TemplateConfigResponseList templateConfigResponseList = new();
            try
            {
                templateConfigResponseList = templateConfigService.GetEnumData(typeof(StackMode));                
            }
            catch (Exception ex)
            {
                Log.Error("ReviewStatusResponse stackMode error {@Error}", ex.Message); 
                templateConfigResponseList.Error();
            }
            return templateConfigResponseList;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Util;

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
        /// 依搜尋條件獲得圖片群組資料列表
        /// </summary>
        /// <param name="imageGroupQuery">圖片群組分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public SealMappingConfigResponsePage Get([FromQuery]SealMappingConfigQuery imageGroupQuery)
        {
            try
            {
                return sealMappingConfigService.GetSealMappingConfigResponsePage(imageGroupQuery);
            }
            catch
            {
                Response response = ResponseUtil.InternalServerError();
                return new SealMappingConfigResponsePage()
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
                return sealMappingConfigService.GetImageGroup(id);
            }
            catch
            {
                Response response = ResponseUtil.InternalServerError();
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
        /// <param name="imageGroupViewModel">群組資料</param>
        /// <returns></returns>
        [HttpPost]
        public Response Post(SealMappingConfigViewModel imageGroupViewModel)
        {
            try
            {
                return sealMappingConfigService.CreateImageGroup(imageGroupViewModel);
            }
            catch
            {
                return ResponseUtil.InternalServerError();
            }
        }

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="imageGroupViewModel">群組資料</param>       
        [HttpPut]
        public Response Put(SealMappingConfigViewModel imageGroupViewModel)
        {
            try
            {
                return sealMappingConfigService.UpdateImageGroup(imageGroupViewModel);
            }
            catch
            {
                return ResponseUtil.InternalServerError();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Services.Accountant;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Accountant;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理圖片群組
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImageGroupController : ControllerBase
    {
        /// <summary>
        /// 宣告會計師資料處理的interface
        /// </summary>
        protected readonly ImageGroupService imageGroupService;

        /// <summary>
        /// 回應結果
        /// </summary>
        protected readonly ResponseService responseService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="imageGroupService">圖片群組</param>
        /// <param name="responseService">回傳結果</param>
        public ImageGroupController(ImageGroupService imageGroupService, ResponseService responseService)
        {
            this.imageGroupService = imageGroupService;
            this.responseService = responseService;
        }



        /// <summary>
        /// 依搜尋條件獲得圖片群組資料列表
        /// </summary>
        /// <param name="imageGroupQuery">圖片群組分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public ImageGroupResponsePage GetImageGroupPage([FromQuery]ImageGroupQuery imageGroupQuery)
        {
            try
            {
                return imageGroupService.GetimageGroupResponsePage(imageGroupQuery);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
                return new ImageGroupResponsePage()
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
        public ImageGroupResponse Get(int id)
        {
            try
            {
                return imageGroupService.GetImageGroup(id);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
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
        public Response Post(ImageGroupViewModel imageGroupViewModel)
        {
            try
            {
                return imageGroupService.CreateImageGroup(imageGroupViewModel);
            }
            catch
            {
                return responseService.Get(ResponseCode.InternalServerError);
            }
        }

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="imageGroupViewModel">群組資料</param>       
        [HttpPut]
        public Response Put(ImageGroupViewModel imageGroupViewModel)
        {
            try
            {
                return imageGroupService.UpdateImageGroup(imageGroupViewModel);
            }
            catch
            {
                return responseService.Get(ResponseCode.InternalServerError);
            }
        }
    }
}

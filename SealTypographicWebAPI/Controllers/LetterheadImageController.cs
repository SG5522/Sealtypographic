using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Services;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理信頭圖片
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    [Authorize(Roles = KeycloakRoleConsts.DATAMANAGE_LETTERHEAD)]
    public class LetterheadImageController : APIControllerBase
    {
        /// <summary>
        /// 管理信頭圖片service
        /// </summary>
        private readonly ILetterheadImageService letterheadImageService;

        /// <summary>
        /// 建構:注入管理信頭圖片service
        /// </summary>
        /// <param name="letterheadImageService">信頭圖片管理service</param>
        /// <param name="applicationUserService"></param>
        public LetterheadImageController(ILetterheadImageService letterheadImageService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.letterheadImageService = letterheadImageService;
        }

        /// <summary>
        /// 取得信頭圖案狀態列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public LetterheadImageStatusResponse StatusList() => letterheadImageService.GetStatus();

        /// <summary>
        /// 取得信頭名稱與圖片建立日期
        /// </summary>
        /// <param name="letterheadId">信頭Id</param>
        /// <returns></returns>
        [HttpGet("{letterheadId}")]
        public async Task<LetterheadImageCreateDateViews> NameAndCreateDate(int letterheadId) 
            => await letterheadImageService.GetNameAndCreateDate(letterheadId, await GetUserId());

        /// <summary>
        /// 取得信頭圖片
        /// </summary>
        /// <param name="id">信頭圖片Id</param>
        /// <param name="isTransparent">是否白底透明化</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LetterheadImageViewModel> ImageViewModel(int id, bool isTransparent) 
            => await letterheadImageService.GetImageViewModel(id, isTransparent, await GetUserId());

        /// <summary>
        /// 新增信頭圖片
        /// </summary>
        /// <param name="letterheadImageForm">信頭圖片</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(LetterheadImageForm letterheadImageForm) 
            => await letterheadImageService.New(letterheadImageForm, await GetUserId());

        /// <summary>
        /// 異動信頭圖片
        /// </summary>
        /// <param name="letterheadImageUpdate">異動信頭圖片資料</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseViewModel> Update(LetterheadImageUpdate letterheadImageUpdate) 
            => await letterheadImageService.Update(letterheadImageUpdate, await GetUserId());      
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.LetterheadImageTemplate;
using SealTypographicWebAPI.Services;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 信頭樣板管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = KeycloakRoleConsts.TEMPLATE_LETTERHEADIMAGETEMPLATE)]
    public class LetterheadImageTemplateController : APIControllerBase
    {
        private readonly ILetterheadImageTemplateService letterheadImageTemplateService;

        /// <summary>
        /// 建構 注入Service
        /// </summary>
        /// <param name="letterheadImageTemplateService"></param>
        /// <param name="applicationUserService"></param>
        public LetterheadImageTemplateController(ILetterheadImageTemplateService letterheadImageTemplateService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.letterheadImageTemplateService = letterheadImageTemplateService;
        }

        /// <summary>
        /// 信頭樣板詳細
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<LetterheadImageTemplateDetailViewModel> Detail(int id)
            => await letterheadImageTemplateService.GetDetail(id, await GetUserId());

        /// <summary>
        /// 信頭簽印樣板圖片顯示
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]/{id}")]
        public async Task<LetterheadImageTemplateImageView> ViewImage(int id)
            => await letterheadImageTemplateService.GetImage(id, await GetUserId());

        /// <summary>
        /// 信頭樣板分頁列表
        /// </summary>
        /// <param name="letterheadTemplateSearch">信頭樣板分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LetterheadImageTemplatePaginate> Paginate([FromQuery] LetterheadImageTemplateSearch letterheadTemplateSearch)
            => await letterheadImageTemplateService.GetPaginate(letterheadTemplateSearch, await GetUserId());

        /// <summary>
        /// 新增信頭樣板
        /// </summary>
        /// <param name="letterheadTemplateForm"></param>        
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(LetterheadImageTemplateForm letterheadTemplateForm)
            => await letterheadImageTemplateService.New(letterheadTemplateForm, await GetUserId());

        /// <summary>
        /// 更新信頭樣板
        /// </summary>
        /// <param name="letterheadTemplateUpdateForm">信頭樣板</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseViewModel> Update(LetterheadImageTemplateUpdateForm letterheadTemplateUpdateForm)
            => await letterheadImageTemplateService.Update(letterheadTemplateUpdateForm, await GetUserId());

        /// <summary>
        /// 刪除信頭樣板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResponseViewModel> Delete(int id)
            => await letterheadImageTemplateService.Delete(id, await GetUserId());
    }
}

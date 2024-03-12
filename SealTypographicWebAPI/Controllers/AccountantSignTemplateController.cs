using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Services;
using Serilog;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 會計師簽印樣板管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountantSignTemplateController : APIControllerBase
    {
        private readonly IAccountantSignTemplateService accountantSignTemplateService;

        /// <summary>
        /// 建構 注入Service
        /// </summary>
        /// <param name="accountantSignTemplateService"></param>
        /// <param name="applicationUserService"></param>
        public AccountantSignTemplateController(IAccountantSignTemplateService accountantSignTemplateService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.accountantSignTemplateService = accountantSignTemplateService;
        }

        /// <summary>        
        /// 會計師簽印樣板詳細資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<AccountantSignTemplateDetailViewModel> Detail(int id)
            => await accountantSignTemplateService.GetDetail(id, await GetUserId());

        /// <summary>
        /// 會計師簽印樣板圖片顯示
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]/{id}")]
        public async Task<AccountantSignTemplateImageView> ViewImage(int id)
            => await accountantSignTemplateService.GetImage(id, await GetUserId());

        /// <summary>
        /// 會計師簽印樣板分頁列表
        /// </summary>
        /// <param name="accountantSignTemplateSearch">會計師簽印樣板分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AccountantSignTemplatePaginate> Paginate([FromQuery] AccountantSignTemplateSearch accountantSignTemplateSearch)
            => await accountantSignTemplateService.GetPaginate(accountantSignTemplateSearch, await GetUserId());

        /// <summary>
        /// 新增會計師簽印樣板
        /// </summary>
        /// <param name="accountantSignTemplateForm">會計師簽印樣板</param>        
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(AccountantSignTemplateForm accountantSignTemplateForm)
            => await accountantSignTemplateService.New(accountantSignTemplateForm, await GetUserId());

        /// <summary>
        /// 更新會計師簽印樣板
        /// </summary>
        /// <param name="accountantSignTemplateUpdateForm">會計師簽印樣板</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseViewModel> Update(AccountantSignTemplateUpdateForm accountantSignTemplateUpdateForm)
            => await accountantSignTemplateService.Update(accountantSignTemplateUpdateForm);

        /// <summary>
        /// 刪除會計師簽印樣板
        /// </summary>
        /// <param name="id">會計師簽印樣板Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResponseViewModel> Delete(int id)
            => await accountantSignTemplateService.Delete(id, await GetUserId());
    }
}

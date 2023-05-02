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
    public class AccountantSignTemplateController : ControllerBase
    {
        private readonly IAccountantSignTemplateService accountantSignTemplateService;

        /// <summary>
        /// 建構 注入Service
        /// </summary>
        /// <param name="accountantSignTemplateService"></param>
        public AccountantSignTemplateController(IAccountantSignTemplateService accountantSignTemplateService)
        {
            this.accountantSignTemplateService = accountantSignTemplateService;
        }

        /// <summary>        
        /// 會計師簽印樣板詳細資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public AccountantSignTemplateDetailViewModel Detail(int id)
        {
            AccountantSignTemplateDetailViewModel accountantSignTemplateDetailViewModel = new();
            try
            {
                Log.Information("AccountantSignTemplate detail input {@Input}", id);
                accountantSignTemplateDetailViewModel = accountantSignTemplateService.GetDetail(id);
                Log.Information("AccountantSignTemplate detail output {@Output}", id);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignTemplate paginate error {@Error}", ex);
                accountantSignTemplateDetailViewModel.DbError();
            }
            return accountantSignTemplateDetailViewModel;
        }

        /// <summary>
        /// 會計師簽印樣板圖片顯示
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]/{id}")]
        public AccountantSignTemplateImageView ViewImage(int id)
        {
            AccountantSignTemplateImageView viewImage = new();
            try
            {
                Log.Information("AccountantSignTemplate viewImage input {@Input}", id);
                viewImage = accountantSignTemplateService.GetImage(id);
                Log.Information("AccountantSignTemplate viewImage output {@Output}", id);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignTemplate viewImage error {@Error}", ex);
                viewImage.DbError();
            }
            return viewImage;
        }

        /// <summary>
        /// 會計師簽印樣板分頁列表
        /// </summary>
        /// <param name="accountantSignTemplateSearch">會計師簽印樣板分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantSignTemplatePaginate Paginate([FromQuery] AccountantSignTemplateSearch accountantSignTemplateSearch)
        {
            AccountantSignTemplatePaginate accountantSignTemplatePaginate = new ();
            try
            {
                Log.Information("AccountantSignTemplate paginate input {@Input}", accountantSignTemplateSearch);
                accountantSignTemplatePaginate = accountantSignTemplateService.GetPaginate(accountantSignTemplateSearch);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignTemplate paginate error {@Error}", ex);
                accountantSignTemplatePaginate.DbError();
            }
            return accountantSignTemplatePaginate;
        }

        /// <summary>
        /// 新增會計師簽印樣板
        /// </summary>
        /// <param name="accountantSignTemplateForm">會計師簽印樣板</param>        
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(AccountantSignTemplateForm accountantSignTemplateForm)
        {
            ResponseViewModel response = new ();
            try
            {                
                Log.Information("AccountantSignTemplate new input {@Input}", accountantSignTemplateForm);
                response = await accountantSignTemplateService.New(accountantSignTemplateForm);
                Log.Information("AccountantSignTemplate new output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignTemplate new error {@Error}", ex);
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 更新會計師簽印樣板
        /// </summary>
        /// <param name="accountantSignTemplateUpdateForm">會計師簽印樣板</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseViewModel> Update(AccountantSignTemplateUpdateForm accountantSignTemplateUpdateForm)
        {
            ResponseViewModel response = new();
            try
            {                
                Log.Information("AccountantSignTemplate update input {@input}", accountantSignTemplateUpdateForm);
                response = await accountantSignTemplateService.Update(accountantSignTemplateUpdateForm);
                Log.Information("AccountantSignTemplate update output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignTemplate update error {@Error}", ex);
                response.DbError();
            }

            return response;
        }

        /// <summary>
        /// 刪除會計師簽印樣板
        /// </summary>
        /// <param name="id">會計師簽印樣板Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ResponseViewModel Delete(int id)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSignTemplate delete input  {@id}", id);
                response = accountantSignTemplateService.Delete(id);
                Log.Information("AccountantSignTemplate delete output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantSignTemplate new error {@Error}", ex);
                response.DbError();
            }

            return response;
        }
    }
}

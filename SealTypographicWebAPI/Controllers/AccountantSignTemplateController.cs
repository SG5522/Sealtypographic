using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 會計師簽印樣板管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountantSignTemplateController : ControllerBase
    {
        

        /// <summary>
        /// 建構 注入Service
        /// </summary>
        
        public AccountantSignTemplateController()
        {
            
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
        /// <param name="accountantSignTemplateForm"></param>        
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New([FromForm]AccountantSignTemplateForm accountantSignTemplateForm)
        {
            ResponseViewModel response = new ();
            try
            {                
                Log.Information("AccountantSignTemplate new input {@Input}", accountantSignTemplateForm);                
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
        public async Task<ResponseViewModel> Update([FromForm]AccountantSignTemplateUpdateForm accountantSignTemplateUpdateForm)
        {
            ResponseViewModel response = new();
            try
            {                
                Log.Information("AccountantSignTemplate update input {@input}", accountantSignTemplateUpdateForm);
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ResponseViewModel Delete(int id)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("AccountantSignTemplate delete input  {@id}", id);
                //response = await AccountantSignTemplateService.Update(AccountantSignTemplateUpdateForm);
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

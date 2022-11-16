using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Accountant;
using SealTypographicWebAPI.Services.Customer;
using SealTypographicWebAPI.Models.Customer;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師基本資料
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AccountantDataController : ControllerBase
    {
        /// <summary>
        /// 宣告會計師資料處理的interface
        /// </summary>
        protected readonly IAccountantService accountantService;

        /// <summary>
        /// 回應結果
        /// </summary>
        protected readonly ResponseService responseService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="accountantService">管理會計師資料</param>
        /// <param name="responseService">回傳結果</param>
        public AccountantDataController(IAccountantService accountantService, ResponseService responseService)
        {
            this.accountantService = accountantService;
            this.responseService = responseService;
        }

        /// <summary>
        /// 依搜尋條件獲得會計師資料列表
        /// </summary>
        /// <param name="accountantQueryPage">會計師分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantResponses GetAccountViewModels([FromQuery]AccountantQueryPage accountantQueryPage)
        {
            try
            {
                return accountantService.GetAccountantViewModels(accountantQueryPage);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
                return new AccountantResponses()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 取得會計師基本資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public AccountantResponse Get(string id)
        {
            try
            {
                return accountantService.GetAccountant(id);
            }
            catch
            {
                Response response = responseService.Get(ResponseCode.InternalServerError);
                return new AccountantResponse()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 建立會計師基本資料
        /// </summary>
        /// <param name="accountantBaseData"></param>
        [HttpPost]
        public Response Post(AccountantBaseData accountantBaseData)
        {
            try
            {
                return accountantService.CreateAccountant(accountantBaseData);
            }
            catch
            {
                return responseService.Get(ResponseCode.InternalServerError);
            }
        }

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="accountantBaseData"></param>        
        [HttpPut]
        public Response Put(AccountantBaseData accountantBaseData)
        {
            try
            {
                return accountantService.UpdateAccountant(accountantBaseData);
            }
            catch
            {
                return responseService.Get(ResponseCode.InternalServerError);
            }
        }

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="accountantId"></param>        
        [HttpDelete("{accountantId}")]
        public Response Delete(string accountantId)
        {
            try
            {
                return accountantService.DeleteAccountant(accountantId);
            }
            catch
            {
                return responseService.Get(ResponseCode.InternalServerError);
            }
        }
    }
}

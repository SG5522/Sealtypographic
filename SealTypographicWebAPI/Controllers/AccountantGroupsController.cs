using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.DbModels;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師群組
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AccountantGroupsController : ControllerBase
    {
        /// <summary>
        /// 宣告會計師資料處理的interface
        /// </summary>
        protected readonly IAccountantGroupService accountantGroupService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="accountantGroupService">管理會計師群組資料</param>
        public AccountantGroupsController(IAccountantGroupService accountantGroupService)
        {
            this.accountantGroupService = accountantGroupService;
        }

        /// <summary>
        /// 依搜尋條件獲得會計師資料列表
        /// </summary>
        /// <param name="accountantGroupQueryPage">會計師群組分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantGroupResponses GetAccountGroupViewModels([FromQuery]AccountantGroupQueryPage accountantGroupQueryPage)
        {
            try
            {
                return accountantGroupService.GetAccountantGroups(accountantGroupQueryPage);
            }
            catch
            {
                Response response = ResponseUtil.InternalServerError();
                return new AccountantGroupResponses()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 取得會計師群組資料
        /// </summary>
        /// <param name="id">群組ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public AccountantGroupResponse Get(string id)
        {
            try
            {
                return accountantGroupService.GetAccountantGroup(id);
            }
            catch
            {
                Response response = ResponseUtil.InternalServerError();
                return new AccountantGroupResponse()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 建立會計師群組
        /// </summary>
        /// <param name="accountantGroupData">群組資料</param>
        [HttpPost]
        public Response Post(AccountantGroupData accountantGroupData)
        {
            try
            {
                return accountantGroupService.CreateAccountantGroup(accountantGroupData);
            }
            catch
            {
                return ResponseUtil.InternalServerError();
            }
        }

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="accountantGroupData">群組資料</param>       
        [HttpPut]
        public Response Put(AccountantGroupData accountantGroupData)
        {
            try
            {
                return accountantGroupService.UpdateAccountantGroup(accountantGroupData);
            }
            catch
            {
                return ResponseUtil.InternalServerError();
            }
        }

        /// <summary>
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupDataId">會計師群組ID</param>
        /// <returns></returns>
        [HttpDelete("{accountantGroupDataId}")]
        public Response Delete(string accountantGroupDataId)
        {
            try
            {
                return accountantGroupService.DeleteAccountantGroup(accountantGroupDataId);
            }
            catch
            {
                return ResponseUtil.InternalServerError();
            }
        }
    }
}

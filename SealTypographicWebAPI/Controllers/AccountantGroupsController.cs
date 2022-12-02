using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Services;
using Serilog;
using SealTypographicWebAPI.Models.AccountantGroup;

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
        public AccountantGroupResponses GetAccountGroupViewModels([FromQuery]AccountantGroupSearch accountantGroupQueryPage)
        {
            try
            {
                Log.Information("AccountantGroups get accountantGroupDatas input {@Input}", accountantGroupQueryPage);
                AccountantGroupResponses accountantGroupResponses = accountantGroupService.GetAccountantGroups(accountantGroupQueryPage);
                Log.Information("AccountantGroups get accountantGroupDatas putput {@Output}", accountantGroupResponses);
                return accountantGroupResponses;
                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get accountantGroupDatas error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.InternalServerError();                
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
                Log.Information("AccountantGroups get accountantGroupDatas input {@Input}", id);
                AccountantGroupResponse accountantGroupResponse = accountantGroupService.GetAccountantGroup(id);
                Log.Information("AccountantGroups get accountantGroupDatas putput {@Output}", accountantGroupResponse);
                return accountantGroupResponse;                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get accountantGroupData error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.InternalServerError();
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
        public ResponseViewModel Post(AccountantGroupData accountantGroupData)
        {
            try
            {
                Log.Information("AccountantGroups post accountantGroupData input {@Input}", accountantGroupData);
                ResponseViewModel response = accountantGroupService.CreateAccountantGroup(accountantGroupData);
                Log.Information("AccountantGroups post accountantGroupData output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups post accountantGroupData error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="accountantGroupData">群組資料</param>       
        [HttpPut]
        public ResponseViewModel Put(AccountantGroupData accountantGroupData)
        {
            try
            {
                Log.Information("AccountantGroups put accountantGroupData input {@Input}", accountantGroupData);
                ResponseViewModel response = accountantGroupService.UpdateAccountantGroup(accountantGroupData);
                Log.Information("AccountantGroups put accountantGroupData output {@Output}", response);
                return response;                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups put accountantGroupData error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }

        /// <summary>
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupDataId">會計師群組ID</param>
        /// <returns></returns>
        [HttpDelete("{accountantGroupDataId}")]
        public ResponseViewModel Delete(string accountantGroupDataId)
        {
            try
            {
                Log.Information("AccountantGroups delete accountantGroupData input {@Input}", accountantGroupDataId);
                ResponseViewModel response = accountantGroupService.DeleteAccountantGroup(accountantGroupDataId);
                Log.Information("AccountantGroups delete accountantGroupData output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups delete accountantGroupData error {@Error}", ex);
                return ResponseUtil.InternalServerError();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Services;
using Serilog;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.Accountant;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師群組
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AccountantGroupController : ControllerBase
    {
        /// <summary>
        /// 會計師群組管理的interface
        /// </summary>
        protected readonly IAccountantGroupService accountantGroupService;

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="accountantGroupService">管理會計師群組interface</param>        
        public AccountantGroupController(IAccountantGroupService accountantGroupService)
        {
            this.accountantGroupService = accountantGroupService;
        }

        /// <summary>
        /// 取得會計師群組所有資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("List")]
        public AccountantGroupList GetAccountantGroupList()
        {
            try
            {                
                AccountantGroupList accountantGroupList = accountantGroupService.GetAccountantGroupList();
                Log.Information("AccountantGroups get accountantGroupList output {@Output}", accountantGroupList);
                return accountantGroupList;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get accountantGroupList error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
                return new()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
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
                Log.Information("AccountantGroups get accountantGroupDatas output {@Output}", accountantGroupResponses);
                return accountantGroupResponses;
                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get accountantGroupDatas error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();                
                return new AccountantGroupResponses()
                {
                    Code = response.Code,
                    Message = response.Message,
                };
            }
        }

        /// <summary>
        /// 取得會計師群組資料(單筆)
        /// </summary>
        /// <param name="accountantGroupId">群組ID</param>
        /// <returns></returns>
        [HttpGet("{accountantGroupId}")]        
        public AccountantGroupResponse Get(int accountantGroupId)
        {
            try
            {
                Log.Information("AccountantGroups get accountantGroupDatas input {@Input}", accountantGroupId);
                AccountantGroupResponse accountantGroupResponse = accountantGroupService.GetAccountantGroupData(accountantGroupId);
                Log.Information("AccountantGroups get accountantGroupDatas output {@Output}", accountantGroupResponse);
                return accountantGroupResponse;                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get accountantGroupForm error {@Error}", ex);
                ResponseViewModel response = ResponseUtil.DBError();
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
        /// <param name="accountantGroupForm">群組資料</param>
        [HttpPost]
        public ResponseViewModel Post(AccountantGroupForm accountantGroupForm)
        {
            try
            {
                Log.Information("AccountantGroups post accountantGroupForm input {@Input}", accountantGroupForm);
                ResponseViewModel response = accountantGroupService.CreateAccountantGroup(accountantGroupForm);
                Log.Information("AccountantGroups post accountantGroupForm output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups post accountantGroupForm error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="accountantGroupFormUpdate">群組資料</param>       
        [HttpPut]
        public ResponseViewModel Put(AccountantGroupFormUpdate accountantGroupFormUpdate)
        {
            try
            {
                Log.Information("AccountantGroups put accountantGroupForm input {@Input}", accountantGroupFormUpdate);
                ResponseViewModel response = accountantGroupService.UpdateAccountantGroup(accountantGroupFormUpdate);
                Log.Information("AccountantGroups put accountantGroupForm output {@Output}", response);
                return response;                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups put accountantGroupForm error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }

        /// <summary>
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupDataId">會計師群組ID</param>
        /// <returns></returns>
        [HttpDelete("{accountantGroupDataId}")]
        public ResponseViewModel Delete(int accountantGroupDataId)
        {
            try
            {
                Log.Information("AccountantGroups delete accountantGroupForm input {@Input}", accountantGroupDataId);
                ResponseViewModel response = accountantGroupService.DeleteAccountantGroup(accountantGroupDataId);
                Log.Information("AccountantGroups delete accountantGroupForm output {@Output}", response);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups delete accountantGroupForm error {@Error}", ex);
                return ResponseUtil.DBError();
            }
        }
    }
}

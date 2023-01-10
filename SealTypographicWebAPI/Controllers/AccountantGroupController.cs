using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
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
            AccountantGroupList accountantGroupList = new ();
            try
            {                
                accountantGroupList = accountantGroupService.GetAll();
                Log.Information("AccountantGroups get(List) output {@Output}", accountantGroupList);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get(List) error {@Error}", ex);
                accountantGroupList.DbError();                
            }
            return accountantGroupList;
        }

        /// <summary>
        /// 依搜尋條件獲得會計師資料列表
        /// </summary>
        /// <param name="accountantGroupQueryPage">會計師群組分頁搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantGroupResponses GetAccountGroupViewModels([FromQuery]AccountantGroupSearch accountantGroupQueryPage)
        {
            AccountantGroupResponses accountantGroupResponses = new();
            try
            {
                Log.Information("AccountantGroups get FromQuery input {@Input}", accountantGroupQueryPage);
                accountantGroupResponses = accountantGroupService.Get(accountantGroupQueryPage);
                Log.Information("AccountantGroups get FromQuery output {@Output}", accountantGroupResponses);                
                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get accountantGroupDatas error {@Error}", ex);                
                accountantGroupResponses.DbError();                
            }
            return accountantGroupResponses;
        }

        /// <summary>
        /// 取得會計師群組資料(單筆)
        /// </summary>
        /// <param name="accountantGroupId">群組ID</param>
        /// <returns></returns>
        [HttpGet("{accountantGroupId}")]        
        public AccountantGroupResponse Get(int accountantGroupId)
        {
            AccountantGroupResponse accountantGroupResponse = new ();
            try
            {
                Log.Information("AccountantGroups get{accountantGroupId} input {@Input}", accountantGroupId);
                accountantGroupResponse = accountantGroupService.GetAccountantData(accountantGroupId);
                Log.Information("AccountantGroups get{accountantGroupId} output {@Output}", accountantGroupResponse);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get{accountantGroupId} error {@Error}", ex);                
                accountantGroupResponse.DbError();                
            }
            return accountantGroupResponse;
        }                
      
        /// <summary>
        /// 建立會計師群組
        /// </summary>
        /// <param name="accountantGroupForm">群組資料</param>
        [HttpPost]
        public ResponseViewModel Post(AccountantGroupForm accountantGroupForm)
        {
            ResponseViewModel response = new ();
            try
            {
                Log.Information("AccountantGroups post input {@Input}", accountantGroupForm);
                response = accountantGroupService.Create(accountantGroupForm);
                Log.Information("AccountantGroups post output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups post error {@Error}", ex);
                response.DbError();                
            }
            return response;
        }

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="accountantGroupFormUpdate">群組資料</param>       
        [HttpPut]
        public ResponseViewModel Put(AccountantGroupFormUpdate accountantGroupFormUpdate)
        {
            ResponseViewModel response = new ();
            try
            {
                Log.Information("AccountantGroups put accountantGroupForm input {@Input}", accountantGroupFormUpdate);
                response = accountantGroupService.Update(accountantGroupFormUpdate);
                Log.Information("AccountantGroups put accountantGroupForm output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups put accountantGroupForm error {@Error}", ex);
                response.DbError();                
            }
            return response;
        }

        /// <summary>
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupDataId">會計師群組ID</param>
        /// <returns></returns>
        [HttpDelete("{accountantGroupDataId}")]
        public ResponseViewModel Delete(int accountantGroupDataId)
        {
            ResponseViewModel response = new ();
            try
            {
                Log.Information("AccountantGroups delete input {@Input}", accountantGroupDataId);
                response = accountantGroupService.Delete(accountantGroupDataId);
                Log.Information("AccountantGroups delete output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups delete error {@Error}", ex);
                response.DbError();                
            }
            return response;
        }
    }
}

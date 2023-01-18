using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using Serilog;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Services.Implements;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師群組
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    public class AccountantGroupController : ControllerBase
    {
        /// <summary>
        /// 管理會計師群組的Service
        /// </summary>
        private readonly IAccountantGroupService accountantGroupService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="accountantGroupService">管理會計師群組的Service</param>        
        public AccountantGroupController(IAccountantGroupService accountantGroupService)
        {
            this.accountantGroupService = accountantGroupService;
        }

        /// <summary>
        /// 取得會計師群組所有資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantGroupList List()
        {
            AccountantGroupList accountantGroupList = new ();            
            try
            {                
                accountantGroupList = accountantGroupService.GetAll();
                Log.Information("AccountantGroups get List output {@Output}", accountantGroupList);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get List error {@Error}", ex);
                accountantGroupList.DbError();                
            }
            return accountantGroupList;
        }

        /// <summary>
        /// 依搜尋條件獲得會計師資料列表(分頁)
        /// </summary>
        /// <param name="accountantGroupSearch">會計師群組搜尋條件(分頁)</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantGroupResponses Paginate([FromQuery]AccountantGroupSearch accountantGroupSearch)
        {
            AccountantGroupResponses accountantGroupResponses = new();
            try
            {
                Log.Information("AccountantGroups get paginate input {@Input}", accountantGroupSearch);
                accountantGroupResponses = accountantGroupService.GetPaginate(accountantGroupSearch);
                Log.Information("AccountantGroups get paginate output {@Output}", accountantGroupResponses);                
                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get paginate error {@Error}", ex);                
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
        public AccountantGroupResponse Data(int accountantGroupId)
        {
            AccountantGroupResponse accountantGroupResponse = new ();
            try
            {
                Log.Information("AccountantGroups get data input {@Input}", accountantGroupId);
                accountantGroupResponse = accountantGroupService.GetData(accountantGroupId);
                Log.Information("AccountantGroups get data output {@Output}", accountantGroupResponse);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups get data error {@Error}", ex);                
                accountantGroupResponse.DbError();                
            }
            return accountantGroupResponse;
        }                
      
        /// <summary>
        /// 新增會計師群組
        /// </summary>
        /// <param name="accountantGroupForm">群組資料</param>
        [HttpPost]
        public ResponseViewModel New(AccountantGroupForm accountantGroupForm)
        {
            ResponseViewModel response = new ();
            try
            {
                Log.Information("AccountantGroups new input {@Input}", accountantGroupForm);
                response = accountantGroupService.New(accountantGroupForm);
                Log.Information("AccountantGroups new output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups new error {@Error}", ex);
                response.DbError();                
            }
            return response;
        }

        /// <summary>
        /// 更新會計師群組資料
        /// </summary>
        /// <param name="accountantGroupFormUpdate">群組資料</param>       
        [HttpPut]
        public ResponseViewModel Update(AccountantGroupFormUpdate accountantGroupFormUpdate)
        {
            ResponseViewModel response = new ();
            try
            {
                Log.Information("AccountantGroups update input {@Input}", accountantGroupFormUpdate);
                response = accountantGroupService.Update(accountantGroupFormUpdate);
                Log.Information("AccountantGroups update output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups update error {@Error}", ex);
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

using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using Serilog;
using SealTypographicWebAPI.Models.AccountantGroup;


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
        /// 取得群組所有資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantGroupList List()
        {
            AccountantGroupList accountantGroupList = new ();            
            try
            {                
                accountantGroupList = accountantGroupService.GetAll();
                Log.Information("AccountantGroups List output {@Output}", accountantGroupList);                
            }
            catch (Exception ex)
            {
                Log.Error("AccountantGroups List error {@Error}", ex.Message);
                accountantGroupList.DbError();                
            }
            return accountantGroupList;
        }

        /// <summary>
        /// 依搜尋條件取得群組列表(分頁)
        /// </summary>
        /// <param name="accountantGroupSearch">群組搜尋條件(分頁)</param>
        /// <returns></returns>
        [HttpGet]
        public AccountantGroupPaginateViewModel Paginate([FromQuery]AccountantGroupSearch accountantGroupSearch) => accountantGroupService.GetPaginate(accountantGroupSearch);

        /// <summary>
        /// 取得群組資料(單筆)
        /// </summary>
        /// <param name="accountantGroupId">群組ID</param>
        /// <returns></returns>
        [HttpGet("{accountantGroupId}")]        
        public AccountantGroupResponse Data(int accountantGroupId) => accountantGroupService.GetData(accountantGroupId);

        /// <summary>
        /// 新增群組
        /// </summary>
        /// <param name="accountantGroupForm">群組資料</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseViewModel New(AccountantGroupForm accountantGroupForm) => accountantGroupService.New(accountantGroupForm);

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="accountantGroupFormUpdate">群組資料(含Id)</param>       
        /// <returns></returns>
        [HttpPut]
        public ResponseViewModel Update(AccountantGroupUpdateForm accountantGroupFormUpdate) => accountantGroupService.Update(accountantGroupFormUpdate);

        /// <summary>
        /// 刪除群組
        /// </summary>
        /// <param name="accountantGroupDataId">群組Id</param>
        /// <returns></returns>
        [HttpDelete("{accountantGroupDataId}")]
        public ResponseViewModel Delete(int accountantGroupDataId) => accountantGroupService.Delete(accountantGroupDataId);
    }
}

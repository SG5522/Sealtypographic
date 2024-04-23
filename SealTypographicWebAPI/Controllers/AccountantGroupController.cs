using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models.AccountantGroup;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 管理會計師群組
    /// </summary>
    [Route("api/[controller]")]    
    [ApiController]
    [A]
    public class AccountantGroupController : APIControllerBase
    {
        /// <summary>
        /// 管理會計師群組的Service
        /// </summary>
        private readonly IAccountantGroupService accountantGroupService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="accountantGroupService">管理會計師群組的Service</param>
        /// <param name="applicationUserService"></param>        
        public AccountantGroupController(IAccountantGroupService accountantGroupService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.accountantGroupService = accountantGroupService;
        }

        /// <summary>
        /// 取得群組所有資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantGroupList List() => accountantGroupService.GetAll();

        /// <summary>
        /// 依搜尋條件取得群組列表(分頁)
        /// </summary>
        /// <param name="accountantGroupSearch">群組搜尋條件(分頁)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AccountantGroupPaginateViewModel> Paginate([FromQuery]AccountantGroupSearch accountantGroupSearch) 
            => await accountantGroupService.GetPaginate(accountantGroupSearch, await GetUserId());

        /// <summary>
        /// 取得群組資料(單筆)
        /// </summary>
        /// <param name="accountantGroupId">群組ID</param>
        /// <returns></returns>
        [HttpGet("{accountantGroupId}")]        
        public async Task<AccountantGroupResponse> Data(int accountantGroupId) => await accountantGroupService.GetData(accountantGroupId, await GetUserId());

        /// <summary>
        /// 新增群組
        /// </summary>
        /// <param name="accountantGroupForm">群組資料</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel> New(AccountantGroupForm accountantGroupForm) => await accountantGroupService.New(accountantGroupForm, await GetUserId());

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="accountantGroupFormUpdate">群組資料(含Id)</param>       
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseViewModel> Update(AccountantGroupUpdateForm accountantGroupFormUpdate) 
            => await accountantGroupService.Update(accountantGroupFormUpdate, await GetUserId());

        /// <summary>
        /// 刪除群組
        /// </summary>
        /// <param name="accountantGroupDataId">群組Id</param>
        /// <returns></returns>
        [HttpDelete("{accountantGroupDataId}")]
        public async Task<ResponseViewModel> Delete(int accountantGroupDataId) => await accountantGroupService.Delete(accountantGroupDataId, await GetUserId());
    }
}

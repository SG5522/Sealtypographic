using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 載入
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    []
    public abstract class APIControllerBase : ControllerBase
    {

        private readonly IApplicationUserService applicationUserService;

        /// <summary>
        /// 建置
        /// </summary>
        protected APIControllerBase(IApplicationUserService applicationUserService)
        {
            this.applicationUserService = applicationUserService;
        }

        /// <summary>
        /// 取得UserId
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected virtual async Task<int> GetUserId() => (await GetUserInfo()).UserId;

        /// <summary>
        /// 取得User資料
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected virtual async Task<UserInfo> GetUserInfo() => await applicationUserService.GetUserInfo(User);
    }
}

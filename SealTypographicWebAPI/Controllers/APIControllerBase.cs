using DBEntities;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using System.Security.Claims;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 載入
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class APIControllerBase : ControllerBase
    {

        private readonly IApplicationUserService applicationUserService; 

        /// <summary>
        /// 建置
        /// </summary>
        public APIControllerBase(IApplicationUserService applicationUserService)
        {
            this.applicationUserService = applicationUserService;
        }

        /// <summary>
        /// 取得User資料
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public virtual async Task<UserInfo> GetUserInfo()
        {                        
            return await applicationUserService.GetUserInfo(User);
        }
    }
}

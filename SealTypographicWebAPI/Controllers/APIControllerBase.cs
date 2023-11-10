using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using System.Security.Claims;

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
        /// <summary>
        /// 取得UserName
        /// </summary>
        protected string UserName => User.Identity?.Name?.ToString() ?? $"You are not logged in";

        /// <summary>
        /// UserId
        /// </summary>
        protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? $"You are not logged in";

        /// <summary>
        /// 使用者資訊
        /// </summary>
        protected UserInfo UserInfo => new()
        {
            UserId = 0,
            UserName = User.Identity?.Name?.ToString() ?? $"You are not logged in",
            KeycloakId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? $"You are not logged in"
        };
    }
}

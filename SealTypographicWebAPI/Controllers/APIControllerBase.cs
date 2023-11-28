using DBEntities;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
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

        private readonly SealTypographicDbContext dbContext; 

        /// <summary>
        /// 建置
        /// </summary>
        public APIControllerBase(SealTypographicDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 取得User資料
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public virtual UserInfo GetUserInfo()
        {
            UserInfo userInfo = new();
            string userName = User.Identity?.Name?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                userInfo = new()
                {
                    UserId = dbContext.Users.FirstOrDefault(x => x.UserName == userName)!.Id,
                    UserName = userName,
                    KeycloakId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? $"You are not logged in"
                };
            }
            return userInfo;
        }
    }
}

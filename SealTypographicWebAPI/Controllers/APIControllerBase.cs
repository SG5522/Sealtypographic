using DBEntities;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
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

        private readonly SealTypographicDbContext dbContext;
        private string UserName;
        private readonly int UserId;        

        /// <summary>
        /// 建置
        /// </summary>
        public APIControllerBase(SealTypographicDbContext dbContext)
        {
            this.dbContext = dbContext;
            UserName = User.Identity?.Name?.ToString() ?? $"You are not logged in";
            if(!string.IsNullOrWhiteSpace(UserName))
            {
                UserId = dbContext.Users.FirstOrDefault(x => x.UserName == UserName)!.Id;
            }                    
        }

        /// <summary>
        /// 取得UserName
        /// </summary>
        //protected string UserName => User.Identity?.Name?.ToString() ?? $"You are not logged in";

        /// <summary>
        /// 使用者資訊
        /// </summary>
        protected UserInfo UserInfo => new()
        {
            UserId = UserId,
            UserName = UserName,            
            KeycloakId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? $"You are not logged in"
        };
    }
}

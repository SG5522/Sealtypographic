using DJKeycloakLib.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 登入驗證
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly KeycloakOptions keycloakOptions;

        /// <summary>
        /// 
        /// </summary>
        public LoginController(IOptionsMonitor<KeycloakOptions> keyCloakOptionsMonitor) 
        {
            keycloakOptions = keyCloakOptionsMonitor.CurrentValue;
        }

        /// <summary>
        /// 登入並獲得驗證
        /// </summary>
        /// <param name="userid">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        [HttpGet("{userid}/{password}")]
        public string Login(string userid, string password)
        {
            var json = new
            {
                userid,
                password,
            };
            return JsonSerializer.Serialize(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public KeycloakOptions AuthUrl()
        {
            return keycloakOptions;
        }
    }
}

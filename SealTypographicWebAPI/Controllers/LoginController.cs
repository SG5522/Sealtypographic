using Microsoft.AspNetCore.Mvc;
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
    }
}

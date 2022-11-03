using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Service;
using SealTypographicWebAPI.Models;
using Newtonsoft.Json;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 登入驗證
    /// </summary>
    [Route("[controller]")]
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
            return JsonConvert.SerializeObject(json);
        }
    }
}

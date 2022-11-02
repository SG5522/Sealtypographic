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
        /// <param name="loginData">帳號密碼</param>
        /// <returns></returns>
        [HttpPost]
        public string Login(LoginData loginData)
        {
            var json = new
            {
                loginData.UserID,
                loginData.Password,
            };
            return JsonConvert.SerializeObject(json);
        }
    }
}

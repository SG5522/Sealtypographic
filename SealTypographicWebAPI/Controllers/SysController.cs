using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 系統訊息
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SysController : ControllerBase
    {
        private readonly ILogger<SysController> logger;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="logger"></param>
        public SysController(ILogger<SysController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 簡易測試系統狀態
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Hello()
        {
            logger.LogDebug("Hello Begin");
            string result = string.Format("Server Run OK. Ver. {0}", typeof(SysController).Assembly.GetName().Version);
            logger.LogDebug("Hello End");
            return result;
        }

        /// <summary>
        /// 取得ASPNETCOREENVIRONMENT的環境變數
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public string EnvironmentVariable()
        {

            logger.LogDebug("Check EnvironmentVariable");
            string result;
            string? aspnetcoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");            

            if (aspnetcoreEnvironment != string.Empty)
            {
                result = $"{"ASPNETCORE_ENVIRONMENT :"}{aspnetcoreEnvironment}";
            }
            else
            {
                result = $"{"ENVIRONMENT :"}{Environment.GetEnvironmentVariable("environment")}";
            }
            
            logger.LogDebug("Check EnvironmentVariable End");
            return result;
        }
    }
}

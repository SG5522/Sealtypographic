using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Keycloak.AuthServices.Authentication;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 系統訊息
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SysController : ControllerBase
    {
        private readonly ILogger<SysController> logger;
        private readonly IWebHostEnvironment environment;
        private readonly KeycloakAuthenticationOptions keycloakAuthenticationOptions;
        //private readonly ClaimsPrincipal claimsPrincipal;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="keycloakAuthenticationOptions"></param>   
        public SysController(ILogger<SysController> logger, IWebHostEnvironment environment, KeycloakAuthenticationOptions keycloakAuthenticationOptions)
        //    ClaimsPrincipal claimsPrincipal)
        {
            this.logger = logger;
            this.environment = environment;
            this.keycloakAuthenticationOptions = keycloakAuthenticationOptions;
            //this.claimsPrincipal = claimsPrincipal;
        }

        /// <summary>
        /// 簡易測試系統狀態
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Hello()
        {
            logger.LogDebug("Hello Begin");
            string result = string.Format(
                                            "Server Run OK. Ver. {0} ProjectName {1}",
                                            Assembly.GetExecutingAssembly().GetName().Version?.ToString(), 
                                            Assembly.GetExecutingAssembly().GetName().Name?.ToString()
                                         );             
            logger.LogDebug("Hello End");
            return result;
        }

        /// <summary>
        /// 取得ENVIRONMENT的環境變數
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public string EnvironmentName()
        {
            logger.LogDebug("Check EnvironmentName");
            string result = $"{"ENVIRONMENT :"}{environment.EnvironmentName}";            
            logger.LogDebug("Check EnvironmentName End");
            return result;
        }

        /// <summary>
        /// 取得Keycloak參數
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public KeycloakAuthenticationOptions Keycloak()
        {
            return keycloakAuthenticationOptions;
        }

        ///// <summary>
        ///// 取得Keycloak參數
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("[Action]")]
        //public string UserName()
        //{            
        //    return claimsPrincipal.Identity.Name;
        //}
    }
}

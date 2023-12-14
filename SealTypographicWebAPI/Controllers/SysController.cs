using Microsoft.AspNetCore.Mvc;
using Keycloak.AuthServices.Authentication;
using System.Reflection;
using SealTypographicWebAPI.Models;
using DBEntities;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 系統訊息
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [AllowAnonymous]
    public class SysController : APIControllerBase
    {
        private readonly ILogger<SysController> logger;
        private readonly IWebHostEnvironment environment;
        private readonly KeycloakAuthenticationOptions keycloakAuthenticationOptions;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="keycloakAuthenticationOptions"></param>
        /// <param name="dbContext"></param>   
        public SysController(ILogger<SysController> logger, 
            IWebHostEnvironment environment, 
            KeycloakAuthenticationOptions keycloakAuthenticationOptions,
            SealTypographicDbContext dbContext) : base(dbContext)
        {
            this.logger = logger;
            this.environment = environment;
            this.keycloakAuthenticationOptions = keycloakAuthenticationOptions;            
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
                                            "Server Run OK. \n" +
                                            "Ver. {0} \n" +
                                            "ProjectName: {1} \n" +
                                            "ENVIRONMENT: {2} \n" ,                                            
                                            Assembly.GetExecutingAssembly().GetName().Version?.ToString(), 
                                            Assembly.GetExecutingAssembly().GetName().Name?.ToString(),
                                            environment.EnvironmentName                                            
                                         );             
            logger.LogDebug("Hello End");
            return result;
        }

        //TODO: 之後需要刪除
        /// <summary>
        /// 取得Keycloak登入配置        
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public LoginConfig KeycloakConfig()
        {
            return new LoginConfig()
            {
                Url = keycloakAuthenticationOptions.AuthServerUrl,
                Realm = keycloakAuthenticationOptions.Realm,
                ClientId = keycloakAuthenticationOptions.Resource
            };                
        }
    }
}

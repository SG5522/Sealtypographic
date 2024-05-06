using Microsoft.AspNetCore.Mvc;
using Keycloak.AuthServices.Authentication;
using System.Reflection;
using SealTypographicWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using SealTypographicWebAPI.Services;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 系統訊息
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]    
    [AllowAnonymous]
    public class SysController : APIControllerBase
    {
        private readonly ILogger<SysController> logger;
        private readonly IWebHostEnvironment environment;
        private KeycloakAuthenticationOptions keycloakAuthenticationOptions;
        private SystemConfigOption systemConfigOption;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="keycloakAuthenticationOptionsMonitor"></param>
        /// <param name="applicationUserService"></param>        
        /// <param name="systemConfigOptionMonitor"></param>
        public SysController(ILogger<SysController> logger, 
            IWebHostEnvironment environment,
            IOptionsMonitor<KeycloakAuthenticationOptions> keycloakAuthenticationOptionsMonitor,
            IOptionsMonitor<SystemConfigOption> systemConfigOptionMonitor,
            IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.logger = logger;
            this.environment = environment;
            keycloakAuthenticationOptions = keycloakAuthenticationOptionsMonitor.CurrentValue;    
            systemConfigOption = systemConfigOptionMonitor.CurrentValue;

            keycloakAuthenticationOptionsMonitor.OnChange(options =>
            {
                keycloakAuthenticationOptions = options;
            });

            systemConfigOptionMonitor.OnChange(options =>
            {
                systemConfigOption = options;
            });
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

        /// <summary>
        /// 取得SystemConfig配置        
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public SystemConfigOption SystemConfig()
        {
            return systemConfigOption;
        }

        /// <summary>
        /// 取得KeycloakUserId
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<string> GetKeyclaokUserId() => (await GetUserInfo()).KeycloakUserId;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<UserInfo> UserInfo() => await GetUserInfo();
    }    
}

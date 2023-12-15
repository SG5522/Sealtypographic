using SealTypographicWebAPI.Models;
using AutoMapper;
using DBEntities;
using System.Security.Claims;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 上傳檔案管理
    /// </summary>
    public class ApplicationUserService : IApplicationUserService
    {        
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 建構
        /// </summary>        
        /// <param name="dbContext">注入資料庫</param>
        /// <param name="mapper"></param>       
        public ApplicationUserService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
        }

        /// <summary>
        /// 取得上傳類別
        /// </summary>
        /// <returns></returns>
        public UserInfo GetUserInfo(ClaimsPrincipal claims)
        {            
            UserInfo userInfo = new();
            if(claims.Identity!.IsAuthenticated)
            {
                var test = dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == claims.Identity.Name);
                userInfo.ApplicationUserId = dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == claims.Identity.Name)?.Id ?? 0 ;
                userInfo.UserName = claims.Identity.Name;
                userInfo.FirstName = claims.FindFirstValue(ClaimTypes.GivenName);
            }
            
            return userInfo;
        }        
    }
}

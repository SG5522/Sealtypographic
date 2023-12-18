using SealTypographicWebAPI.Models;
using AutoMapper;
using DBEntities;
using System.Security.Claims;
using DBEntities.Entities;
using DJKeycloakLib.Models.BaseModel;
using DJKeycloakAPI.Models.Users;
using System.Data.Common;

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
                userInfo.ApplicationUserId = dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == claims.Identity.Name)?.Id ?? 0 ;
                userInfo.UserName = claims.Identity.Name;
                userInfo.FirstName = claims.FindFirstValue(ClaimTypes.GivenName);
            }            
            return userInfo;
        }

        /// <summary>
        /// 取得上傳類別
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel> AddUser(NewUserForm newUserForm, string username = "admin")
        {
            ResponseModel response = new();

            try
            {
                //TODO:之後公司資料表由登入的adminUser取得帳號資料在反找所屬公司。                
                //Company company = dbContext.Companys.First(x => x.ApplicationUsers.Any(x => x.UserName == username));
                Company company = dbContext.Companys.First(x => x.Id == 1);

                ApplicationUser user = new()
                {
                    UserName = newUserForm.Username,
                    Company = company
                };

                dbContext.ApplicationUsers.Add(user);
                await dbContext.SaveChangesAsync();
                response = ResponseModel.Success();
            }
            catch(DbException ex)
            {
                response = ResponseModel.SystemError();
                response.Message = ex.InnerException!.Message;
            }
            catch(Exception ex) 
            {
                response = ResponseModel.SystemError();
                response.Message = ex.Message;                
            }

            return response;
        }
    }
}

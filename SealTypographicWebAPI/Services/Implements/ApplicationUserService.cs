using SealTypographicWebAPI.Models;
using AutoMapper;
using DBEntities;
using System.Security.Claims;
using DBEntities.Entities;
using DJKeycloakLib.Models.BaseModel;
using DJKeycloakAPI.Models.Users;
using System.Data.Common;
using DBEntities.Entities.AccountantModels;
using Serilog;

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
        private readonly ILogger<ApplicationUserService> logger;

        /// <summary>
        /// 建構
        /// </summary>        
        /// <param name="dbContext">注入資料庫</param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>       
        public ApplicationUserService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<ApplicationUserService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        /// <summary>
        /// 取得上傳類別
        /// </summary>
        /// <returns></returns>
        public async Task<UserInfo> GetUserInfo(ClaimsPrincipal claims)
        {            
            UserInfo userInfo = new();
            if(claims.Identity!.IsAuthenticated)
            {                
                ApplicationUser? applicationUser = dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == claims.Identity.Name);
                if(applicationUser != null)
                {
                    userInfo.ApplicationUserId = applicationUser.Id;
                }
                else
                {
                    await AddUser(new NewUserForm
                    {
                        Username = claims.Identity.Name!,
                        FirstName = claims.FindFirstValue(ClaimTypes.GivenName),
                        LastName = claims.FindFirstValue(ClaimTypes.GivenName),
                    });
                }
                userInfo.UserName = claims.Identity.Name!;
                userInfo.FirstName = claims.FindFirstValue(ClaimTypes.GivenName);
            }            
            return userInfo;
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUserForm"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ResponseModel> AddUser(NewUserForm newUserForm, string userName = "admin")
        {
            logger.LogInformation("AddUser input newUserForm: {@accountantId} userName {@userName}", newUserForm, userName);

            ResponseModel response = new();

            try
            {
                //TODO:之後公司資料表由登入的adminUser取得帳號資料在反找所屬公司。                
                //Company company = dbContext.Companys.First(x => x.ApplicationUsers.Any(x => x.UserName == username));
                Company company = dbContext.Companys.First(x => x.Id == 1);

                ApplicationUser user = new()
                {
                    UserName = newUserForm.Username,
                    FirstName = newUserForm.FirstName,
                    LastName = newUserForm.LastName,
                    Email = newUserForm.Email,
                    Company = company
                };

                dbContext.ApplicationUsers.Add(user);
                await dbContext.SaveChangesAsync();
                response = ResponseModel.Success();
            }
            catch(DbException ex)
            {
                logger.LogError("AddUser Error while updating database {@error}", ex.InnerException!.Message);
                response = ResponseModel.SystemError();
                response.Message = ex.InnerException!.Message;
            }
            catch(Exception ex) 
            {
                logger.LogError("AddUser error {@error}", ex.Message);
                response = ResponseModel.SystemError();
                response.Message = ex.Message;                
            }
            return response;
        }
    }
}

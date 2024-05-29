using SealTypographicWebAPI.Models;
using AutoMapper;
using DBEntities;
using System.Security.Claims;
using DBEntities.Entities;
using DJKeycloakLib.Models.BaseModel;
using DJKeycloakAPI.Models.Users;
using System.Data.Common;
using DBEntities.Utils;
using System.Text.Json;
using SealTypographicWebAPI.Consts;
using Microsoft.EntityFrameworkCore;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 上傳檔案管理
    /// </summary>
    public class ApplicationUserService : IApplicationUserService
    {        
        private readonly SealTypographicDbContext dbContext;        
        private readonly ILogger<ApplicationUserService> logger;

        /// <summary>
        /// 建構
        /// </summary>        
        /// <param name="dbContext">注入資料庫</param>        
        /// <param name="logger"></param>       
        public ApplicationUserService(SealTypographicDbContext dbContext, ILogger<ApplicationUserService> logger)
        {
            this.dbContext = dbContext;                       
            this.logger = logger;
        }

        /// <summary>
        /// 取得使用者資訊
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
                    userInfo.UserId = applicationUser.Id;
                    //如果資料庫跟keycloak上的名稱不同就進行同步更新
                    if(applicationUser.FirstName != claims.FindFirstValue(ClaimTypes.GivenName) 
                        || applicationUser.LastName == claims.FindFirstValue(ClaimTypes.Surname))
                    {
                        applicationUser.FirstName = claims.FindFirstValue(ClaimTypes.GivenName);
                        applicationUser.LastName = claims.FindFirstValue(ClaimTypes.Surname);
                        await dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    await AddUser(new NewUserForm
                    {
                        Username = claims.Identity.Name!,
                        FirstName = claims.FindFirstValue(ClaimTypes.GivenName),
                        LastName = claims.FindFirstValue(ClaimTypes.Surname),
                    });
                }
                userInfo.KeycloakUserId = claims.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
                userInfo.UserName = claims.Identity.Name!;
                userInfo.FirstName = claims.FindFirstValue(ClaimTypes.GivenName);
                userInfo.LastName = claims.FindFirstValue(ClaimTypes.Surname);                                
                userInfo.SetRoles(claims);
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
                Company company = new();
                            
                if (userName == "admin")
                {
                    company = dbContext.Companys.Include(x => x.ApplicationUsers).First(x => x.Id == 1);
                }
                else
                {
                    company = dbContext.Companys.Include(x => x.ApplicationUsers).First(x => x.ApplicationUsers.Any(x => x.UserName == userName));
                }

                ApplicationUser user = new()
                {
                    UserName = newUserForm.Username,
                    FirstName = newUserForm.FirstName,
                    LastName = newUserForm.LastName,
                    Email = newUserForm.Email,                    
                    CreateDate = DateTime.Now,
                    CreateUserId = 1
                };

                company.ApplicationUsers.Add(user);
                await dbContext.SaveChangesAsync();
                response = ResponseModel.Success();
            }
            catch(DbException ex)
            {
                logger.LogError("AddUser Error while updating database {@error}", ex.InnerException?.Message);
                response = ResponseModel.SystemError();
                response.Message = ex.InnerException?.Message;
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

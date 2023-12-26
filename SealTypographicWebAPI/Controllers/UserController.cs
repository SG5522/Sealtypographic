using AutoMapper;
using DBEntities;
using DBEntities.Entities;
using DJKeycloakAPI.Controllers;
using DJKeycloakAPI.Models.Users;
using DJKeycloakLib.Models.BaseModel;
using DJKeycloakLib.Services;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 帳號管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AdminUserController
    {           
        private readonly IApplicationUserService applicationUserService;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="adminService"></param>
        /// <param name="applicationUserService"></param>
        public UserController(IMapper mapper, IAdminService adminService, IApplicationUserService applicationUserService) : base(mapper, adminService)
        {
            this.applicationUserService = applicationUserService;
        }

        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="newUserForm"></param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public override async Task<ResponseModel> Post(NewUserForm newUserForm)
        {
            ResponseModel responseModel = await base.Post(newUserForm);

            if (responseModel.Code == KeycloakResponseCode.Success)
            {
                UsersQuery userQuery = new()
                {
                   Username = newUserForm.Username,                    
                };
                
                ResponseModel<UserViewModelPaginate> userViewModels = await base.Get(userQuery);
                UserViewModel? userViewModel = userViewModels.Data!.Users!.FirstOrDefault();
                if (userViewModel != null)
                {
                    responseModel =  await applicationUserService.AddUser(newUserForm);
                }
            }
            return responseModel;
        }
    }
}

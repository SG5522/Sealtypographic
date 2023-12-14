using AutoMapper;
using DBEntities;
using DBEntities.Entities;
using DJKeycloakAPI.Models.Users;
using DJKeycloakLib.Models.BaseModel;
using DJKeycloakLib.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 帳號管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : DJKeycloakAPI.Controllers.AdminUserController
    {
        private readonly SealTypographicDbContext dbContext;        

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="adminService"></param>
        /// <param name="dbContext"></param>
        public UserController(IMapper mapper, IAdminService adminService, SealTypographicDbContext dbContext) : base(mapper, adminService)
        {            
            this.dbContext = dbContext;
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
                    Exact = true
                };

                ResponseModel<IList<UserViewModel>> userViewModels = await base.Get(userQuery);
                UserViewModel? userViewModel = userViewModels.Data!.FirstOrDefault();
                if (userViewModel != null)
                {
                    //TODO:之後公司資料表由Keycloak取得帳號資料在反找公司。
                    //Company company = dbContext.Companys.First(x => x.Users.Any(x => x.KeycloakUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    Company company = dbContext.Companys.First(x => x.Id == 1);         

                    ApplicationUser user = new() 
                    {
                        UserName = userViewModel.Username,
                        KeycloakUserId = userViewModel.Id!,
                        Company = company
                    };

                    dbContext.ApplicationUsers.Add(user);
                    dbContext.SaveChanges();
                }
            }
            return responseModel;
        }
    }
}

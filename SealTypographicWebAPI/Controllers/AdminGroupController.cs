using AutoMapper;
using DJKeycloakAPI.Controllers;
using DJKeycloakAPI.Models.Users;
using DJKeycloakLib.Models.BaseModel;
using DJKeycloakLib.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 帳號管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =  KeycloakRoleConsts.SYSTEM_ACCOUNTGROUPMANAGE)]
    public class AdminGroupController : DJKeycloakAPI.Controllers.AdminGroupController
    {           
        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="adminService"></param>
        public AdminGroupController(IMapper mapper, IAdminService adminService) : base(mapper, adminService)
        {
        }
    }
}

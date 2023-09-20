using DJKeycloakLib.Model;
using DJKeycloakLib.Model.BaseModel;
using DJKeycloakLib.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KeyCloakAdminController : ControllerBase
    {                
        private readonly IKeycloakAdminService keycloakAdminService;

        /// <summary>
        /// 建置
        /// </summary>
        public KeyCloakAdminController(IKeycloakAdminService keyCloakAdminService)
        {
            this.keycloakAdminService = keyCloakAdminService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserDetailResponse> UserData()
        {
            UserDetailResponse userDetailResponse = new ();
            try
            {                
                userDetailResponse = await keycloakAdminService.GetUserData(User.FindFirstValue(ClaimTypes.NameIdentifier));               
            }
            catch (Exception ex)
            {
                userDetailResponse.Message = ex.Message;
            }
            return userDetailResponse;   
        }

        /// <summary>
        /// 取得此Client的Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<ClientRolesResponse> ClientRoles()
        {
            return await keycloakAdminService.GetRoles(); ;
        }

        /// <summary>
        /// 取得使用者分頁資料
        /// </summary>
        /// <param name="userSearch">Keycloak User搜尋條件</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<UserDataPaginate> UserDataPaginate([FromQuery] UserSearch userSearch)
        {
            return await keycloakAdminService.GetUserDataPaginate(userSearch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<string> RoleMappings(string groupId)
        {
            return await keycloakAdminService.GroupRoleMappings(groupId); ;
        }


        /// <summary>
        /// 註冊帳號
        /// </summary>
        /// <param name="userData"></param>        
        [HttpPost]
        public async Task<ResponseBaseModel> New([FromBody] UserDataForm userData)
        {
            ResponseBaseModel response = new ();
            try
            {
                response = await keycloakAdminService.New(userData);
            }
            catch (Exception ex)
            {
                response.SystemError();
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="credentials"></param>
        [HttpPut("{userId}")]
        public async Task<ResponseBaseModel> ResetPassword(string userId, [FromBody] Credentials credentials)
        {
            ResponseBaseModel response = new();
            try
            {
                response = await keycloakAdminService.ResetPassword(userId ,credentials);
            }
            catch (Exception ex)
            {
                response.SystemError();
                response.Message = ex.Message;
            }
            return response;
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}

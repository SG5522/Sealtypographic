using Azure.Identity;
using DBEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Keycloak;
using SealTypographicWebAPI.Models.KeyCloak;
using SealTypographicWebAPI.Services;
using Serilog;
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
        private readonly IKeycloakAdminService keyCloakAdminService;

        /// <summary>
        /// 建置
        /// </summary>
        public KeyCloakAdminController(IKeycloakAdminService keyCloakAdminService)
        {
            this.keyCloakAdminService = keyCloakAdminService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<KeycloakUserDetailResponse> UserData()
        {
            KeycloakUserDetailResponse keycloakUserDetailResponse = new ();
            try
            {                
                keycloakUserDetailResponse = await keyCloakAdminService.GetUserData(User.FindFirstValue(ClaimTypes.NameIdentifier));               
            }
            catch (Exception ex)
            {
                keycloakUserDetailResponse.Message = ex.Message;
            }
            return keycloakUserDetailResponse;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<KeycloakClientRolesResponse> Roles()
        {
            KeycloakClientRolesResponse keyCloakClientRolesResponse = new ();
            try
            {
                keyCloakClientRolesResponse = await keyCloakAdminService.GetRoles();                
            }
            catch (Exception ex)
            {
                keyCloakClientRolesResponse.Error();
                keyCloakClientRolesResponse.Message = ex.Message;
            }
            return keyCloakClientRolesResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<string> RoleMappings(string groupId)
        {
            string result = string.Empty;
            try
            {
                result = await keyCloakAdminService.GroupRoleMappings(groupId);
            }
            catch (Exception ex)
            {
                //keyCloakClientRolesResponse.Error();
                //keyCloakClientRolesResponse.Message = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 註冊帳號
        /// </summary>
        /// <param name="keyCloakUserData"></param>        
        [HttpPost]
        public async Task<ResponseViewModel> New([FromBody] KeycloakUserDataForm keyCloakUserData)
        {
            ResponseViewModel response = new ();
            try
            {
                response = await keyCloakAdminService.New(keyCloakUserData);
            }
            catch (Exception ex)
            {
                response.Error();
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="keycloakCredentials"></param>
        [HttpPut("{userId}")]
        public async Task<ResponseViewModel> ResetPassword(string userId, [FromBody] KeycloakCredentials keycloakCredentials)
        {
            ResponseViewModel response = new();
            try
            {
                response = await keyCloakAdminService.ResetPassword(userId ,keycloakCredentials);
            }
            catch (Exception ex)
            {
                response.Error();
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

using Azure.Identity;
using DBEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.KeyCloak;
using SealTypographicWebAPI.Services;
using Serilog;

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
        public async Task<string> Get()
        {
            string response = string.Empty;
            try
            {
                response = await keyCloakAdminService.GetUserData(User.Identity.Name);               
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<KeycloakClientRolesResponse> GetClinetId()
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
        /// 註冊帳號
        /// </summary>
        /// <param name="keyCloakUserData"></param>        
        [HttpPost]
        public async Task<ResponseViewModel> New([FromBody] KeycloakUserData keyCloakUserData)
        {
            //RestRequest request = new("users", Method.Post);
            //request.AddHeader("Content-Type", "application/json");
            //string userData = JsonSerializer.Serialize(keyCloakUserData);
            //request.AddStringBody(userData, DataFormat.Json);
            //RestResponse response = await client.ExecuteAsync(request);
            //return Ok(response.Content);

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

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

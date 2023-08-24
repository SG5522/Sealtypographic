using DBEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models.KeyCloak;
using SealTypographicWebAPI.Utils;
using System.Text.Json;


namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {                
        private readonly RestClient client;
        private readonly KeyCloakAdminOption keyCloakAdminOption;

        /// <summary>
        /// 建置
        /// </summary>
        public AuthController(IOptionsSnapshot<KeyCloakAdminOption> keyCloakAdminOption)
        {
            this.keyCloakAdminOption = keyCloakAdminOption.Value;

            RestClientOptions options = new(this.keyCloakAdminOption.ApiBaseUrl)
            {
                Authenticator = new KeyCloakAuthenticator(keyCloakAdminOption.Value)
            };
            client = new (options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            RestRequest request = new("users",Method.Get);
            RestResponse response = await client.ExecuteGetAsync(request);
            return Ok(response.Content);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult Test()
        {
            User user = new()
            {
                Id = 1,                    
            };
            return Ok(user);
        }

        /// <summary>
        /// 註測帳號
        /// </summary>
        /// <param name="keyCloakUserData"></param>        
        [HttpPost]
        public async Task<IActionResult> New([FromBody] KeyCloakUserData keyCloakUserData)
        {
            RestRequest request = new("users", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            string userData = JsonSerializer.Serialize(keyCloakUserData);
            request.AddStringBody(userData, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            return Ok(response.Content);
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

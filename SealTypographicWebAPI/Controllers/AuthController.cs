using Azure.Core;
using DBEntities;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using RTools_NTS.Util;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private string apiurl = "http://djimage.myftp.org:50001/";
        //private readonly RestClient client;
        private readonly KeycloakAuthenticationOptions keycloakAuthenticationOptions;


        /// <summary>
        /// 建置
        /// </summary>
        public AuthController(KeycloakAuthenticationOptions keycloakAuthenticationOptions)
        {
            this.keycloakAuthenticationOptions = keycloakAuthenticationOptions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            //if (Request.Headers.Authorization.ToString() != null)
            OAuth2AuthorizationRequestHeaderAuthenticator authenticator = new(
                                Request.Headers["Authorization"], "Bearer"
            );
            RestClientOptions options = new(apiurl)
            {
                Authenticator = authenticator
            };
            RestClient client = new RestClient(options);

            RestRequest request = new("api/Sys/Keycloak");
            //request.AddParameter("status", 1);
            RestResponse<KeycloakAuthenticationOptions> response = await client.ExecuteGetAsync<KeycloakAuthenticationOptions>(request);

            return Ok(response.Data);            
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

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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

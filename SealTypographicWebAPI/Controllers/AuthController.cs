using Azure.Core;
using DBEntities;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using RTools_NTS.Util;
using SealTypographicWebAPI.Config;

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
        private readonly RestClient client;

        /// <summary>
        /// 建置
        /// </summary>
        public AuthController()
        {            
            RestClientOptions options = new(apiurl)
            {
                Authenticator = new KeyCloakAuthenticator("http://djimage.myftp.org:50500/realms/djidentity/protocol/openid-connect/token", "webapi", "YTXFZmbRWKIzktPHwinRwd55yqYWRf3F")
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

            ////if (Request.Headers.Authorization.ToString() != null)
            //OAuth2AuthorizationRequestHeaderAuthenticator authenticator = new(
            //                    Request.Headers["Authorization"], "Bearer"
            //);

            //RestClientOptions options = new(apiurl)
            //{
            //    //Authenticator = authenticator
            //    Authenticator = new HttpBasicAuthenticator ("admin","1qaz@WSX")
            //};
            //RestClient client = new RestClient(options);


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

using Azure.Core;
using Keycloak.AuthServices.Authentication;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 管理keyCloak內的帳號內容
    /// </summary>
    public class KeyCloakService
    {
        private readonly KeycloakAuthenticationOptions keycloakAuthenticationOptions;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="keycloakAuthenticationOptions"></param>
        public KeyCloakService(KeycloakAuthenticationOptions keycloakAuthenticationOptions)
        {
            this.keycloakAuthenticationOptions = keycloakAuthenticationOptions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string test()
        {
            RestClientOptions options = new RestClientOptions($"{keycloakAuthenticationOptions.AuthServerUrl}{keycloakAuthenticationOptions.Realm}{"/users/count"}")
            {
                Authenticator = new HttpBasicAuthenticator("admin", "1qaz@WSX3edc")
            };
            RestClient client = new (options);
            RestRequest request = new ("statuses/home_timeline.json");
            //return await client.GetAsync<HomeTimeline>(request, cancellationToken);
            return "";
        }

    }
}

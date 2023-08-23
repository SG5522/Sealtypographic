using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Utils
{
    record TokenResponse
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; init; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KeyCloakAuthenticator : AuthenticatorBase
    {
        private readonly string baseUrl;
        private readonly string clientId;
        private readonly string clientSecret;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public KeyCloakAuthenticator(string baseUrl, string clientId, string clientSecret) : base("")
        {
            this.baseUrl = baseUrl;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        /// <summary>
        /// 覆蓋取得accessToken的方法
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
        {
            Token = string.IsNullOrEmpty(Token) ? await GetToken() : Token;
            return new HeaderParameter(KnownHeaders.Authorization, Token);
        }

        async Task<string> GetToken()
        {
            RestClientOptions options = new(baseUrl);
            //{
            //    Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(clientId, clientSecret),
            //};

            //RestClientOptions options = new(baseUrl)
            //{
            //    Authenticator = new HttpBasicAuthenticator(clientId, clientSecret),
            //};

            RestClient client = new(options);
            RestRequest request = new RestRequest("oauth2/token") { Method = Method.Post }
                .AddHeader("Accept", "application/json")
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddParameter("grant_type", "client_credentials")
                .AddParameter("client_id", clientId)
                .AddParameter("client_secret", clientSecret)
                .AddParameter("username","admin")
                .AddParameter("password", "1qaz@WSX");
            TokenResponse? response = await client.PostAsync<TokenResponse>(request);            
            return $"{response!.TokenType}{response!.AccessToken}";
        }
    }
}

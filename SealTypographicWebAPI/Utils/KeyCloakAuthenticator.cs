using RestSharp;
using RestSharp.Authenticators;
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
    /// KeyCloak驗證器(取得token使用)
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

        /// <summary>
        /// 獲得token
        /// </summary>
        /// <returns></returns>
        async Task<string> GetToken()
        {
            RestClientOptions options = new(baseUrl);

            RestClient client = new(options);
            RestRequest request = new ("protocol/openid-connect/token", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("username", "admin");
            request.AddParameter("password", "1qaz@WSX");
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_Secret", clientSecret);
            TokenResponse? response = await client.PostAsync<TokenResponse>(request);            
            return $"{response!.TokenType} {response!.AccessToken}";
        }
    }
}

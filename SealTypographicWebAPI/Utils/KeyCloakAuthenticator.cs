using RestSharp;
using RestSharp.Authenticators;
using SealTypographicWebAPI.Config;
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
    public class KeycloakAuthenticator : AuthenticatorBase
    {
        private readonly KeycloakAdminOption keyCloakAdmin;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="keyCloakAdminOption"></param>
        public KeycloakAuthenticator(KeycloakAdminOption keyCloakAdminOption) : base("")
        {
            keyCloakAdmin = keyCloakAdminOption;
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
            RestClientOptions options = new(keyCloakAdmin.TokenBaseUrl);

            RestClient client = new(options);
            RestRequest request = new ("protocol/openid-connect/token", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("username", keyCloakAdmin.UserName);
            request.AddParameter("password", keyCloakAdmin.Pwaosrsd);
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", keyCloakAdmin.ClientId);
            request.AddParameter("client_Secret", keyCloakAdmin.ClientSecret);
            TokenResponse? response = await client.PostAsync<TokenResponse>(request);            
            return $"{response!.TokenType} {response!.AccessToken}";
        }
    }
}

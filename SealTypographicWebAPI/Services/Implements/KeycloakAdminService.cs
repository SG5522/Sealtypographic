using Microsoft.Extensions.Options;
using RestSharp;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.KeyCloak;
using SealTypographicWebAPI.Utils;
using System.Text.Json;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 管理keyCloak內的帳號內容
    /// </summary>
    public class KeycloakAdminService : IKeycloakAdminService
    {
        private readonly RestClient client;
        private readonly KeycloakAdminOption keyCloakAdminOption;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="keyCloakAdminOption">讀取appsetting keyCloakAdmin的參數</param>
        public KeycloakAdminService(IOptionsSnapshot<KeycloakAdminOption> keyCloakAdminOption)
        {
            this.keyCloakAdminOption = keyCloakAdminOption.Value;

            RestClientOptions options = new(this.keyCloakAdminOption.ApiBaseUrl)
            {
                Authenticator = new KeycloakAuthenticator(keyCloakAdminOption.Value)
            };
            client = new(options);
        }

        /// <summary>
        /// 取得User資料
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<string> GetUserData(string userName)
        {
            RestRequest request = new(KeycloakAdminConsts.Users, Method.Get);
            request.AddParameter("username", userName);
            request.AddParameter("exact", true);
            RestResponse response = await client.ExecuteGetAsync(request);
            return response.Content;
        }

        public async Task<string> GetRoles(string clientId)
        {
            RestRequest request = new(KeycloakAdminConsts.Clinets, Method.Get);
            request.AddParameter("clientId", clientId);
            RestResponse response = await client.ExecuteGetAsync(request);
            return response.Content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyCloakUserData"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(KeyCloakUserData keyCloakUserData)
        {
            ResponseViewModel response = new();
            RestRequest request = new(KeycloakAdminConsts.Users, Method.Post);

            request.AddHeader("Content-Type", "application/json");            
            request.AddStringBody(JsonSerializer.Serialize(keyCloakUserData), DataFormat.Json);
            RestResponse restResponse = await client.ExecuteAsync(request);

            if(restResponse.Content == "")
            {
                response.Success();
            }
            else
            {
                response.Error();
                response.Message = restResponse.Content;
            }
            return response;            
        }
    }
}

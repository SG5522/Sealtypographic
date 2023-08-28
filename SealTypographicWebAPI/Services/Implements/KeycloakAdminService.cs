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
            RestRequest request = new(KeycloakAdminUrlConsts.Users, Method.Get);
            request.AddParameter("username", userName);
            request.AddParameter("exact", true);
            RestResponse response = await client.ExecuteGetAsync(request);
            return response.Content;
        }

        /// <summary>
        /// 取得client的role資料
        /// </summary>
        /// <returns></returns>
        public async Task<KeyCloakClientRolesResponse> GetRoles()
        {
            KeyCloakClientRolesResponse keyCloakClientRolesResponse = new();
            RestRequest request = new(KeycloakAdminUrlConsts.Role, Method.Get);
            request.AddUrlSegment("id", keyCloakAdminOption.ResourceId);
            RestResponse response = await client.ExecuteGetAsync(request);
            if(response.IsSuccessful && response.Content!= null)
            {
                List<KeyCloakClientRole>? keyCloakClientRoles = JsonSerializer.Deserialize<List<KeyCloakClientRole>>(response.Content);
                if(keyCloakClientRoles != null)
                {
                    keyCloakClientRolesResponse.KeyCloakClientRoles = keyCloakClientRoles;
                    keyCloakClientRolesResponse.Success();
                }
                else
                {
                    keyCloakClientRolesResponse.Error();
                }
            }

            //List<KeyCloakClientRole>? keyCloakClientRoles = await client.GetJsonAsync<List<KeyCloakClientRole>>(KeycloakAdminUrlConsts.Role, new { id = keyCloakAdminOption.ResourceId });
            //if(keyCloakClientRoles != null)
            //{
            //    keyCloakClientRolesResponse.KeyCloakClientRoles = keyCloakClientRoles;
            //    keyCloakClientRolesResponse.Success();
            //}


            //keyCloakClientRolesResponse.KeyCloakClientRoles = await client.GetJsonAsync<List<KeyCloakClientRole>>(KeycloakAdminUrlConsts.Role);            
            //RestResponse response = await client.ExecuteGetAsync(request);
            

            return keyCloakClientRolesResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyCloakUserData"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(KeyCloakUserData keyCloakUserData)
        {
            ResponseViewModel response = new();
            RestRequest request = new(KeycloakAdminUrlConsts.Users, Method.Post);

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

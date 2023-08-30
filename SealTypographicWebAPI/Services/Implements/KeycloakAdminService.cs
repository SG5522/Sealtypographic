using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Controllers;
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
        private readonly KeycloakAdminOption keycloakAdminOption;
        private readonly ILogger<KeycloakAdminService> logger;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="keyCloakAdminOption">讀取appsetting keyCloakAdmin的參數</param>
        /// <param name="logger">Logger</param>
        public KeycloakAdminService(IOptionsSnapshot<KeycloakAdminOption> keyCloakAdminOption, ILogger<KeycloakAdminService> logger)
        {
            keycloakAdminOption = keyCloakAdminOption.Value;

            RestClientOptions options = new(keycloakAdminOption.ApiBaseUrl)
            {
                Authenticator = new KeycloakAuthenticator(keyCloakAdminOption.Value)
            };
            client = new(options);

            this.logger = logger;
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
        public async Task<KeycloakClientRolesResponse> GetRoles()
        {
            KeycloakClientRolesResponse keycloakClientRolesResponse = new();
            try
            {                
                RestRequest request = new(KeycloakAdminUrlConsts.Role, Method.Get);
                request.AddUrlSegment("id", keycloakAdminOption.ResourceId);
                RestResponse response = await client.ExecuteGetAsync(request);
                if (response.IsSuccessful && response.Content != null)
                {
                    List<KeycloakClientRole>? keycloakClientRoles = JsonSerializer.Deserialize<List<KeycloakClientRole>>(response.Content);
                    if (keycloakClientRoles != null)
                    {
                        keycloakClientRolesResponse.Roles = keycloakClientRoles;
                        keycloakClientRolesResponse.Success();
                        logger.LogInformation("GetRoles Success");
                    }
                    else
                    {
                        keycloakClientRolesResponse.KeycloakNoData();
                        logger.LogError("GetRoles Nodata");
                    }
                }
                else
                {
                    keycloakClientRolesResponse.Error();
                    logger.LogError("GetRoles keycloak Api Get Roles errorMessage {message}", response.ErrorMessage);
                }
            }
            catch (Exception ex) 
            {
                logger.LogError("GetRoles Api Error {message}", ex.Message);
            }
            
            return keycloakClientRolesResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keycloakUserData"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(KeycloakUserData keycloakUserData)
        {
            ResponseViewModel response = new();
            RestRequest request = new(KeycloakAdminUrlConsts.Users, Method.Post);

            request.AddHeader("Content-Type", "application/json");            
            request.AddStringBody(JsonSerializer.Serialize(keycloakUserData), DataFormat.Json);
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

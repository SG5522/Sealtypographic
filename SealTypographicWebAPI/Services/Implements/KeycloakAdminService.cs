using Microsoft.Extensions.Options;
using RestSharp;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Keycloak;
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
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<KeycloakUserDetailResponse> GetUserData(string userId)
        {
            KeycloakUserDetailResponse keycloakUserDetailResponse = new();
            try
            {
                RestRequest request = new(KeycloakAdminUrlConsts.UsersQueryWithId, Method.Get);
                request.AddUrlSegment("id", userId);
                request.AddParameter("exact", true);
                RestResponse response = await client.ExecuteGetAsync(request);
                if (response.IsSuccessful && response.Content != null)
                {
                    KeycloakUserDetailViewModel? keycloakUserDetailViewModel = JsonSerializer.Deserialize<KeycloakUserDetailViewModel>(response.Content);
                    if (keycloakUserDetailViewModel != null)
                    {
                        keycloakUserDetailResponse.KeycloakUserDetail = keycloakUserDetailViewModel;
                        keycloakUserDetailResponse.KeycloakUserDetail.Groups = await GetUserGroupDatas(userId);
                        logger.LogInformation("GetUserData Success");
                    }
                    else
                    {
                        keycloakUserDetailResponse.KeycloakNoData();
                        logger.LogError("GetUserData Nodata");
                    }
                }
                else
                {
                    keycloakUserDetailResponse.Error();
                    logger.LogError("GetUserData keycloak Api Get Roles errorMessage {message}", response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("GetUserData Api Error {message}", ex.Message);
            }
            return keycloakUserDetailResponse;
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
                    List<KeycloakRoleMapping>? keycloakRoleMappings = JsonSerializer.Deserialize<List<KeycloakRoleMapping>>(response.Content);
                    if (keycloakRoleMappings != null)
                    {
                        keycloakClientRolesResponse.RoleMappings = keycloakRoleMappings;
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
        /// 取得群組的角色權限關聯
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<string> GroupRoleMappings(string groupId)
        {
            RestRequest request = new(KeycloakAdminUrlConsts.GroupRoleMapping, Method.Get);
            request.AddUrlSegment("id", groupId);
            request.AddUrlSegment("client", keycloakAdminOption.ResourceId);
            RestResponse response = await client.ExecuteGetAsync(request);
            return response.Content;
        }

        /// <summary>
        /// 新增User
        /// </summary>
        /// <param name="keycloakUserData"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(KeycloakUserDataForm keycloakUserData)
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
        
        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseViewModel> ResetPassword (string userid, KeycloakCredentials keycloakCredentials)
        {
            ResponseViewModel response = new();
            
            RestRequest request = new(KeycloakAdminUrlConsts.ResetPassword, Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddUrlSegment("id", userid);
            request.AddStringBody(JsonSerializer.Serialize(keycloakCredentials), DataFormat.Json);
            RestResponse restResponse = await client.ExecuteAsync(request);

            if (restResponse.Content == "")
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

        /// <summary>
        /// 取得user資料
        /// </summary>
        /// <param name="keycloakUserSearch">Keycloak User搜尋條件</param>
        /// <returns></returns>
        public async Task<KeycloakUserDataPaginate> GetUserDataPaginate(KeycloakUserSearch keycloakUserSearch)
        {
            KeycloakUserDataPaginate result = new();
            try
            {
                RestRequest request = new(KeycloakAdminUrlConsts.Users, Method.Get);
                if(keycloakUserSearch.UserName != null)
                {
                    request.AddParameter("username", keycloakUserSearch.UserName);
                }
                request.AddParameter("first", keycloakUserSearch.PageNumber);
                request.AddParameter("max", keycloakUserSearch.PageSize);
                request.AddParameter("exact", true);
                RestResponse response = await client.ExecuteGetAsync(request);
                if (response.IsSuccessful && response.Content != null)
                {
                    List<KeycloakUserDataViewModel>? keycloakUserDataViewModel = JsonSerializer.Deserialize<List<KeycloakUserDataViewModel>>(response.Content);

                    if (keycloakUserDataViewModel != null)
                    {
                        //user資料 (依條件與分頁限制顯示)
                        result.UserDatas = keycloakUserDataViewModel;
                        //使用者數量與頁數相關
                        result.PageNumber = keycloakUserSearch.PageNumber;
                        result.PageSize = keycloakUserSearch.PageSize;
                        result.TotalCount = await GetUserCount(keycloakUserSearch);
                        result.TotalPage = TotalPageUtil.GetTotalPage(result.TotalCount, keycloakUserSearch.PageSize);
                        result.Success();
                        logger.LogInformation("GetUserData Success");
                    }
                    else
                    {
                        result.KeycloakNoData();
                        logger.LogError("GetUserData Nodata");
                    }
                }
                else
                {
                    result.KeycloakLinkError();
                    logger.LogError("GetUserData keycloak Api errorMessage {message}", response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("GetUserData Error {message}", ex.Message);
            }

            return result;
        }

        private async Task<int> GetUserCount(KeycloakUserSearch keycloakUserSearch)
        {
            RestRequest request = new(KeycloakAdminUrlConsts.UsersCount, Method.Get);
            if (keycloakUserSearch.UserName != null)
            {
                request.AddParameter("username", keycloakUserSearch.UserName);
            }
            request.AddParameter("exact", true);

            RestResponse response = await client.ExecuteGetAsync(request);
            return response.Content != null ? int.Parse(response.Content) : 0 ;
        }

        /// <summary>
        /// 取得該user的群組資料
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private async Task<List<KeycloakUserGroup>> GetUserGroupDatas(string userid)
        {
            List<KeycloakUserGroup> result = new();
            try
            {
                RestRequest request = new(KeycloakAdminUrlConsts.UserGroup, Method.Get);
                request.AddUrlSegment("id", userid);                
                RestResponse response = await client.ExecuteGetAsync(request);
                if (response.IsSuccessful && response.Content != null)
                {
                    List<KeycloakUserGroup>? keycloakUserDataViewModel = JsonSerializer.Deserialize<List<KeycloakUserGroup>>(response.Content);

                    if (keycloakUserDataViewModel != null)
                    {
                        result = keycloakUserDataViewModel;
                        logger.LogInformation("GetUserGroupDatas Success");
                    }
                    else
                    {
                        logger.LogError("GetUserGroupDatas Nodata");
                    }
                }
                else
                {
                    logger.LogError("GetUserGroupDatas keycloak Api errorMessage {message}", response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("GetUserGroupDatas Error {message}", ex.Message);
            }

            return result;
        }
    }
}

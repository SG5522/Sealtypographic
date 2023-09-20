using DJKeycloakLib.Config;
using RestSharp;
using System.Text.Json;
using DJKeycloakLib.Model;
using DJKeycloakLib.Model.BaseModel;
using DJKeycloakLib.Util;
using Microsoft.Extensions.Options;

namespace DJKeycloakLib.Service
{
    /// <summary>
    /// 管理keyCloak內的帳號內容
    /// </summary>
    public class KeycloakAdminService : IKeycloakAdminService
    {
        private readonly RestClient client;
        private readonly KeycloakAdminOption keycloakAdminOption;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="keyCloakAdminOption">讀取appsetting keyCloakAdmin的參數</param>        
        public KeycloakAdminService(IOptionsSnapshot<KeycloakAdminOption> keyCloakAdminOption)
        {
            keycloakAdminOption = keyCloakAdminOption.Value;
            RestClientOptions options = new(keycloakAdminOption.ApiBaseUrl)
            {
                Authenticator = new KeycloakAuthenticator(keyCloakAdminOption.Value)
            };
            client = new(options);
        }

        /// <summary>
        /// 取得User資料
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDetailResponse> GetUserData(string userId)
        {
            UserDetailResponse keycloakUserDetailResponse = new();
            try
            {
                RestRequest request = new(KeycloakAdminUrlConsts.UsersQueryWithId, Method.Get);
                request.AddUrlSegment("id", userId);
                request.AddParameter("exact", true);
                RestResponse response = await client.ExecuteGetAsync(request);
                if (response.IsSuccessful && response.Content != null)
                {
                    UserDetailViewModel? KeycloakUserData = JsonSerializer.Deserialize<UserDetailViewModel>(response.Content);
                    if (KeycloakUserData != null)
                    {
                        keycloakUserDetailResponse.UserDetail = KeycloakUserData;                        
                        keycloakUserDetailResponse.Success();
                    }
                    else
                    {
                        keycloakUserDetailResponse.NoData();
                    }
                }
                else
                {
                    keycloakUserDetailResponse.KeycloakAPIError();
                    keycloakUserDetailResponse.Message = response.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                keycloakUserDetailResponse.Message = ex.Message;
            }
            return keycloakUserDetailResponse;
        }

        /// <summary>
        /// 取得client的role資料
        /// </summary>
        /// <returns></returns>
        public async Task<ClientRolesResponse> GetRoles()
        {
            ClientRolesResponse clientRolesResponse = new();
            try
            {
                RestRequest request = new(KeycloakAdminUrlConsts.Role, Method.Get);
                request.AddUrlSegment("id", keycloakAdminOption.ResourceId);
                RestResponse response = await client.ExecuteGetAsync(request);
                if (response.IsSuccessful && response.Content != null)
                {
                    List<RoleMapping>? keycloakRoleMappings = JsonSerializer.Deserialize<List<RoleMapping>>(response.Content);
                    if (keycloakRoleMappings != null)
                    {
                        clientRolesResponse.RoleMappings = keycloakRoleMappings;
                        clientRolesResponse.Success();
                    }
                    else
                    {
                        clientRolesResponse.NoData();
                    }
                }
                else
                {
                    clientRolesResponse.KeycloakAPIError();
                    clientRolesResponse.Message = response.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                clientRolesResponse.Message = ex.Message;
            }

            return clientRolesResponse;
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
        public async Task<ResponseBaseModel> New(UserDataForm keycloakUserData)
        {
            ResponseBaseModel response = new();
            RestRequest request = new(KeycloakAdminUrlConsts.Users, Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddStringBody(JsonSerializer.Serialize(keycloakUserData), DataFormat.Json);
            RestResponse restResponse = await client.ExecuteAsync(request);

            if (restResponse.Content == "")
            {
                response.Success();
            }
            else
            {
                response.KeycloakAPIError();
                response.Message = restResponse.Content;
            }
            return response;
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseBaseModel> ResetPassword(string userid, Credentials keycloakCredentials)
        {
            ResponseBaseModel response = new();

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
                response.KeycloakAPIError();
                response.Message = restResponse.ErrorMessage;
            }
            return response;
        }

        /// <summary>
        /// 取得user資料
        /// </summary>
        /// <param name="keycloakUserSearch">Keycloak User搜尋條件</param>
        /// <returns></returns>
        public async Task<UserDataPaginate> GetUserDataPaginate(UserSearch keycloakUserSearch)
        {
            UserDataPaginate result = new();
            try
            {
                RestRequest request = new(KeycloakAdminUrlConsts.Users, Method.Get);
                if (keycloakUserSearch.UserName != null)
                {
                    request.AddParameter("username", keycloakUserSearch.UserName);
                }
                request.AddParameter("first", keycloakUserSearch.PageNumber);
                request.AddParameter("max", keycloakUserSearch.PageSize);
                request.AddParameter("exact", true);
                RestResponse response = await client.ExecuteGetAsync(request);
                if (response.IsSuccessful && response.Content != null)
                {
                    List<UserDetailViewModel>? keycloakUserDataViewModel = JsonSerializer.Deserialize<List<UserDetailViewModel>>(response.Content);

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
                    }
                    else
                    {
                        result.NoData();
                    }
                }
                else
                {
                    result.KeycloakAPIError();
                    result.Message = response.ErrorMessage;                                        
                }
            }
            catch (Exception ex)
            {
                result.SystemError();
                result.Message = ex.Message;                
            }

            return result;
        }

        private async Task<int> GetUserCount(UserSearch keycloakUserSearch)
        {
            RestRequest request = new(KeycloakAdminUrlConsts.UsersCount, Method.Get);
            if (keycloakUserSearch.UserName != null)
            {
                request.AddParameter("username", keycloakUserSearch.UserName);
            }
            request.AddParameter("exact", true);

            RestResponse response = await client.ExecuteGetAsync(request);
            return response.Content != null ? int.Parse(response.Content) : 0;
        }

        /// <summary>
        /// 取得該user的群組資料
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<UserGroupsResponse> GetUserGroupDatas(string userid)
        {
            UserGroupsResponse result = new();
            //List<UserGroup> result = new();
            try
            {
                RestRequest request = new(KeycloakAdminUrlConsts.UserGroup, Method.Get);
                request.AddUrlSegment("id", userid);
                RestResponse response = await client.ExecuteGetAsync(request);
                if (response.IsSuccessful && response.Content != null)
                {
                    List<UserGroup>? keycloakUserDataViewModel = JsonSerializer.Deserialize<List<UserGroup>>(response.Content);

                    if (keycloakUserDataViewModel != null)
                    {
                        result.UserGroups = keycloakUserDataViewModel;
                        result.Success();
                    }
                    else
                    {           
                        result.NoData();
                    }
                }
                else
                {
                    result.KeycloakAPIError();
                    result.Message = response.ErrorMessage;                    
                }
            }
            catch (Exception ex)
            {
                result.SystemError();
                result.Message = ex.Message;                
            }

            return result;
        }
    }
}

using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.Keycloak
{
    /// <summary>
    /// 顯示KeycloakUser 詳細資料資料
    /// </summary>
    public class KeycloakUserDetailViewModel : KeycloakUserDataViewModel
    {
        /// <summary>
        /// 所屬群組
        /// </summary>
        public List<KeycloakUserGroup> Groups { get; set; }
    }
}

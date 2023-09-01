namespace SealTypographicWebAPI.Models.Keycloak
{
    /// <summary>
    /// User詳細資料及回應訊息
    /// </summary>
    public class KeycloakUserDetailResponse : ResponseViewModel
    {
        /// <summary>
        /// User詳細資料
        /// </summary>
        public KeycloakUserDetailViewModel KeycloakUserDetail { get; set; }
    }
}

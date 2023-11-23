namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 登入配置
    /// </summary>
    public class LoginConfig
    {
        /// <summary>
        /// 驗證站台網址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Keycloak Realm
        /// </summary>
        public string Realm { get; set; }

        /// <summary>
        /// ClientId
        /// </summary>
        public string ClientId { get; set; }
    }
}

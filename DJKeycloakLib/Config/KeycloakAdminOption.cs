namespace DJKeycloakLib.Config
{
    /// <summary>
    /// KeyCloakAdmin的參數
    /// </summary>
    public class KeycloakAdminOption
    {
        /// <summary>
        /// KeyCloak的Realm
        /// </summary>
        public string Realm { get; set; }

        /// <summary>
        /// 驗證網頁
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 連結的ClientId
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 連結的ClientId對應的Secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Pwaosrsd { get; set; }

        /// <summary>
        /// ClientId的"Id"(guid)
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// KeyCloak的Admin RestApi的Url
        /// </summary>
        public string ApiBaseUrl
        {
            get
            {
                return $"{BaseUrl}{"admin/realms"}/{Realm}/";
            }
        }

        /// <summary>
        /// KeyCloak的Admin的token Url
        /// </summary>
        public string TokenBaseUrl
        {
            get
            {
                return $"{BaseUrl}{"realms"}/{Realm}/";
            }
        }
    }
}

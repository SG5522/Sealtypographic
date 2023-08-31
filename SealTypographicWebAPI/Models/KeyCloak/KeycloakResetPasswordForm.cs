using SealTypographicWebAPI.Models.KeyCloak;

namespace SealTypographicWebAPI.Models.Keycloak
{
    /// <summary>
    /// 
    /// </summary>
    public class KeycloakResetPasswordForm : KeycloakCredentials
    {
        /// <summary>
        /// Keycloak的UserId
        /// </summary>
        public string UserId { get; set; }
    }
}

namespace DJKeycloakLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ResetPasswordForm : Credentials
    {
        /// <summary>
        /// Keycloak的UserId
        /// </summary>
        public string UserId { get; set; }
    }
}

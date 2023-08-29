using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.KeyCloak
{
    /// <summary>
    /// ClinetRole的規則清單
    /// </summary>
    public class KeycloakClientRolesResponse : ResponseViewModel
    {
        /// <summary>
        /// 此Client所有的Role內容
        /// </summary>
        public List<KeycloakClientRole> Roles { get; set; }
    }
}

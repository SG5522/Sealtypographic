using DJKeycloakLib.Model.BaseModel;
using System.Text.Json.Serialization;

namespace DJKeycloakLib.Model
{
    /// <summary>
    /// Keycloak 使用者群組
    /// </summary>
    public class UserGroupsResponse : ResponseBaseModel
    {
        /// <summary>
        /// User詳細資料
        /// </summary>
        public List<UserGroup> UserGroups { get; set; }
    }
}

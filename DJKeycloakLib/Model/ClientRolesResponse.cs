using DJKeycloakLib.Model.BaseModel;

namespace DJKeycloakLib.Model
{
    /// <summary>
    /// ClinetRole的規則清單
    /// </summary>
    public class ClientRolesResponse : ResponseBaseModel
    {
        /// <summary>
        /// 此Client所有的Role內容
        /// </summary>
        public List<RoleMapping> RoleMappings { get; set; }
    }
}

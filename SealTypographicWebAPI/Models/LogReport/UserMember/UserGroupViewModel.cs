using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.LogReport.UserMember
{
    /// <summary>
    /// 群組資料
    /// </summary>
    public class UserGroupViewModel
    {
        /// <summary>
        /// 群組的Guid
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        public string? Name { get; set; }
    }
}

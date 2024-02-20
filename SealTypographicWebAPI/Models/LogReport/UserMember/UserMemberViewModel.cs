using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.LogReport.UserMember
{
    /// <summary>
    /// User資料
    /// </summary>
    public class UserMemberViewModel
    {
        /// <summary>
        /// Guid
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; }

        /// <summary>
        /// 使用者Id
        /// </summary>
        [Display(Order = 0)]
        public string? Username { get; set; }

        /// <summary>
        /// 使用者姓氏
        /// </summary>
        [Display(Order = 1)]
        public string? LastName { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Display(Order = 2)]
        public string? FirstName { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Display(Order = 3)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 群組名稱(多個)
        /// </summary>
        [Display(Order = 4)]
        public List<string> Groups { get; set; }
    }
}

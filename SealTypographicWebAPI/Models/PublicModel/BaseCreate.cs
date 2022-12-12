using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.PublicModel
{
    /// <summary>
    /// 新增更新資料時會填入的資料
    /// </summary>
    public class BaseCreate
    {
        /// <summary>
        /// 創建UserId(暫無帳號先給0)
        /// </summary>
        /// <example>1</example>
        [Required]
        public int CreateUserId { get; set; }

    }
}

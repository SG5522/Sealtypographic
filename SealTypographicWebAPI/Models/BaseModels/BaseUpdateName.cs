using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 新增更新資料時會填入的資料
    /// </summary>
    public class BaseUpdateName : BaseName
    {
        /// <summary>
        /// 更新UserId(暫無帳號先給0)
        /// </summary>
        /// <example>1</example>
        [Required]
        public int UpdateUserId { get; set; }
    }
}

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 新增更新資料時會填入的資料
    /// </summary>
    public class BaseForm
    {
        /// <summary>
        /// 創建UserId(暫無帳號先給0)
        /// </summary>
        /// <example>0</example>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 更新UserId(暫無帳號先給0)
        /// </summary>
        /// <example>0</example>
        public int UpdateUserId { get; set; }
    }
}

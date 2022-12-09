namespace SealTypographicWebAPI.Models.PublicModel
{
    /// <summary>
    /// 新增更新資料時會填入的資料
    /// </summary>
    public class BaseUpdateNameData : BaseNameData
    {
        /// <summary>
        /// 更新UserId(暫無帳號先給0)
        /// </summary>
        /// <example>0</example>
        public int UpdateUserId { get; set; }
    }
}

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 新增更新資料時會填入的資料
    /// </summary>
    public class BaseNameData : BaseData
    {
        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>Name</example>
        public string Name { get; set; }
    }
}

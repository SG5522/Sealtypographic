namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 新增更新資料時會填入的資料
    /// </summary>
    public class BaseName : BaseData
    {
        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>公司名稱 Or 名字</example>
        public string Name { get; set; }
    }
}

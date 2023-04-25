namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 需要使用到Name使用的基本資料
    /// </summary>
    public abstract class BaseName : BaseData
    {
        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>姓名</example>
        public string Name { get; set; }
    }
}

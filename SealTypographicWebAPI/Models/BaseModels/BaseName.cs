namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 需要使用到Name使用的基本資料
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

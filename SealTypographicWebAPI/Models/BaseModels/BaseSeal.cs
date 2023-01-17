namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 印鑑
    /// </summary>
    public class BaseSeal : BaseData
    {       
        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }
    }
}

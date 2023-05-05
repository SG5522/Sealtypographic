namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 印鑑圖像ID 與 位置
    /// </summary>
    public abstract class BaseSealLocationViewModel : BaseLocationModel
    {
        /// <summary>
        /// 圖片
        /// </summary>
        public string ImageBase64 { get; set; }
    }
}

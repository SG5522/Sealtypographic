namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 印鑑圖像ID 與 位置
    /// </summary>
    public abstract class BaseSealLocationViewModel : BaseLocationModel
    {
        /// <summary>
        /// 各印鑑、簽印位置Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 圖片
        /// </summary>
        /// <example>Image/...</example>
        public string ImageBase64 { get; set; }
    }
}

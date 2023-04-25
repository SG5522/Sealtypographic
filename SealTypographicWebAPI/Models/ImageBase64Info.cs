using DBEntities.Consts;

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// ImageBase64資訊
    /// </summary>
    public class ImageBase64Info
    {
        /// <summary>
        /// 印鑑類型
        /// </summary>
        public SealType SealType { get; set; }

        /// <summary>
        /// 客戶、會計、信頭 編號
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Base64圖片字串
        /// </summary>
        public string ImageBase64 { get; set; } = string.Empty;

        /// <summary>
        /// 建檔時間
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}

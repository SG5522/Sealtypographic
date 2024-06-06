using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 客戶印鑑、會計師簽印的縮圖資訊
    /// </summary>
    public class BaseThumbnailSeal
    {        
        /// <summary>
        /// 縮圖字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ThumbnailBase64 { get; set; }

        /// <summary>
        /// 縮圖路徑(Base64)
        /// </summary>
        [JsonIgnore]
        public string ThumbnailFullPath { get; set; }

        /// <summary>
        /// 圖片加密key
        /// </summary>
        [JsonIgnore]
        public string ThumbnailEncryptKey { get; set; }


    }
}

using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 新增客戶印鑑、會計師簽印的基本資料
    /// </summary>
    public class BaseSeal : BaseData
    {        
        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }

        /// <summary>
        /// 圖檔路徑
        /// </summary>
        [JsonIgnore]
        public string ImageFullPath { get; set; }

        /// <summary>
        /// 圖片加密key
        /// </summary>
        [JsonIgnore]
        public string ImageEncryptKey { get; set; }
    }
}

using SealTypographicWebAPI.Utils;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 印鑑圖像ID 與 位置
    /// </summary>
    public abstract class BaseSealLocation : BaseLocation
    {        
        private string imagePath;

        /// <summary>
        /// 圖片
        /// </summary>
        /// <example>Image/...</example>
        public string ImageBase64 { get; set; }

        /// <summary>
        /// 圖片位置
        /// </summary>
        [JsonIgnore]
        public string ImagePath { get; set; }
    }
}

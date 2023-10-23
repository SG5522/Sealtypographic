using DJLib.Models;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 印鑑圖像ID 與 位置
    /// </summary>
    public abstract class BaseSealLocationViewModel : BaseLocationModel
    {
        private string imageBase64;

        /// <summary>
        /// 圖片
        /// </summary>
        /// <example>Image/...</example>
        public string ImageBase64
        {
            get { return imageBase64; }
            set
            {
                imageBase64 = value;
                if (imageBase64 != string.Empty)
                {
                    ImageInfo imageInfo = ImageInfo.FromImageBase64(imageBase64);
                    imageInfo.ReSize(imageInfo, 0.1);
                    ThumbnailImageBase64 = imageInfo.ToBase64();
                }
            }
        }

        /// <summary>
        /// 縮圖
        /// </summary>
        [JsonIgnore]
        public string ThumbnailImageBase64 {get; set;}
    }
}

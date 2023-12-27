using DJImageLib.Extensions;
using DJImageLib.Utils;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 印鑑圖像ID 與 位置
    /// </summary>
    public abstract class BaseSealLocation : BaseLocation
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
                //if (imageBase64 != string.Empty)
                //{
                //    ThumbnailImageBase64 = ImageUtil.ReSize(DataUrlUtil.GetBase64(imageBase64).ToBytes(), 0.1, 0.1);
                //}
            }
        }

        /// <summary>
        /// 縮圖
        /// </summary>
        [JsonIgnore]
        public string ThumbnailImageBase64 {get; set;}
    }
}

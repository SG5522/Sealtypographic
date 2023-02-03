using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace DJLib.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// 圖片
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// 圖片格式
        /// </summary>
        public IImageFormat ImageFormat { get; set; }

    }
}

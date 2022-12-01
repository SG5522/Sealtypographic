using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 圖片處理
    /// </summary>
    public class ImageUtil
    {
        /// <summary>
        /// Base64轉圖
        /// </summary>
        /// <returns></returns>
        public Image Base64ToImage (string imageBase64)
        {
            byte[] bytes = Convert.FromBase64String(imageBase64);            
            return Image.Load(bytes);
        }

        /// <summary>
        /// 圖片轉base64
        /// </summary>
        /// <param name="image"></param>        
        /// <returns></returns>
        public string ImageToBase64(Image image, IImageFormat format)
        {
            return image.ToBase64String(format);
        }
    }
}

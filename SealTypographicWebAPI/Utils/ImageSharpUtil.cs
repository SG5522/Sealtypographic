using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.Drawing.Imaging;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 圖片處理
    /// </summary>
    public class ImageSharpUtil
    {
        /// <summary>
        /// 圖片轉base64
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>        
        /// <returns></returns>
        public static string ImageToBase64(Image image,IImageFormat format)
        {                    
            return image.ToBase64String(format);
        }

        /// <summary>
        /// 檔案路徑圖片轉base64
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string PathImageFileToBase64(string fullPath)
        {
            Image image = Image.Load(fullPath, out IImageFormat format);
            return ImageToBase64(image, format);
        }
    }
}

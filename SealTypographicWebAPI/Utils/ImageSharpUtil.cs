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
        /// Base64轉圖
        /// </summary>
        /// <param name="imageBase64"></param>
        /// <returns></returns>
        public static ResponseViewModel Base64ToSaveImage (string imageBase64)
        {
            string path = @"C:\Users\User\Desktop\test";
            imageBase64 = imageBase64[(imageBase64.IndexOf(",") + 1)..];
            byte[] bytes = Convert.FromBase64String(imageBase64);
            Image image = Image.Load(bytes, out IImageFormat format);
            switch(format.Name)
            {
                case "JPEG":
                    image.SaveAsJpeg(path + ".jpg");
                    return ResponseUtil.Success();
                case "PNG":
                    image.SaveAsJpeg(path + ".png");
                    return ResponseUtil.Success();
                default:
                    return ResponseUtil.InternalServerError();
            }
        }

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

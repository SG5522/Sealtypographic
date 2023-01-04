using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using DJLib.Models;
using System;
using System.IO;

namespace DJLib
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

        /// <summary>
        /// Base64轉圖存檔
        /// </summary>
        /// <param name="ImageBase64">BASE64圖檔字串</param>
        /// <param name="saveImageInfo">存檔資訊</param>        
        /// <returns></returns>
        public static void Base64ToSaveImage(string ImageBase64, SaveImageInfo saveImageInfo)
        {
            string base64string = ImageBase64.Substring(ImageBase64.IndexOf("base64,") + 7);
            byte[] bytes = Convert.FromBase64String(base64string);
            Image image = Image.Load(bytes, out IImageFormat format);
            SaveImageFile(image, format, saveImageInfo);
        }

        /// <summary>
        /// 存檔
        /// </summary>
        /// <param name="image">影像</param>
        /// <param name="format">格式</param>
        /// <param name="saveImageInfo">存檔資訊</param>
        private static void SaveImageFile(Image image, IImageFormat format, SaveImageInfo saveImageInfo)
        {            
            if (!Directory.Exists(saveImageInfo.Folder))
            {
                Directory.CreateDirectory(saveImageInfo.Folder);
            }            
            switch (format.Name)
            {
                case "BMP":
                    saveImageInfo.Filename += ".bmp";
                    image.SaveAsBmp(saveImageInfo.Folder + saveImageInfo.Filename);                    
                    break;
                case "JPEG":
                    saveImageInfo.Filename += ".jpg";
                    image.SaveAsJpeg(saveImageInfo.Folder + saveImageInfo.Filename);
                    break;
                case "PNG":
                    saveImageInfo.Filename += ".png";
                    image.SaveAsPng(saveImageInfo.Folder + saveImageInfo.Filename);
                    break;                                   
            }
        }
    }
}

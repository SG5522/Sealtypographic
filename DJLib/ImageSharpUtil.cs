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

        private static void SaveImageFile(Image image, IImageFormat format, SaveImageInfo saveScanForm)
        {
            if (!Directory.Exists(saveScanForm.Folder))
            {
                Directory.CreateDirectory(saveScanForm.Folder);
            }
            switch (format.Name)
            {
                case "BMP":
                    image.SaveAsBmp(saveScanForm.Folder + saveScanForm.Filename + ".bmp");
                    break;
                case "JPEG":
                    image.SaveAsJpeg(saveScanForm.Folder + saveScanForm.Filename + ".jpg");
                    break;
                case "PNG":
                    image.SaveAsPng(saveScanForm.Folder + saveScanForm.Filename + "png");
                    break;                                   
            }
        }
    }
}

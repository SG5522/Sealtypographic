using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using DJLib.Models;
using System;
using System.IO;
using SixLabors.ImageSharp.Processing;
using System.Drawing.Imaging;

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
        /// Base64轉成Image
        /// </summary>
        /// <param name="ImageBase64">BASE64圖檔字串</param>         
        /// <returns></returns>
        public static ImageInfo Base64ToImageInfo(string ImageBase64)
        {
            ImageInfo imageInfo = new ImageInfo();
            string base64string = ImageBase64.Substring(ImageBase64.IndexOf("base64,") + 7);
            byte[] bytes = Convert.FromBase64String(base64string);

            imageInfo.Image = Image.Load(bytes, out IImageFormat format);
            imageInfo.ImageFormat = format;

            return imageInfo;
        }

        /// <summary>
        /// 調整圖片大小(Image)
        /// </summary>
        /// <param name="image">圖片</param>
        /// <param name="scale">縮放比例 1.00 = 100%  0.01 = 1%</param>        
        public static void ReSize(Image image, double scale)
        {
            int width = (int)(image.Width * scale);
            int height = (int)(image.Height * scale);
            image.Mutate(x => x.Resize(width, height));
        }

        /// <summary>
        /// 調整圖片大小(ImageBase64)
        /// </summary>
        /// <param name="ImageBase64"></param>
        /// <param name="scale">縮放比例 1.00 = 100%  0.01 = 1%</param>        
        public static void ReSize(string ImageBase64, double scale)
        {
            ImageInfo imageInfo = Base64ToImageInfo(ImageBase64);            
            ReSize(imageInfo.Image, scale);
        }

        /// <summary>
        /// 存檔
        /// </summary>
        /// <param name="image">影像</param>
        /// <param name="format">格式</param>
        /// <param name="saveImageInfo">存檔資訊</param>
        public static void SaveFile(Image image, IImageFormat format, SaveFullPath saveImageInfo)
        {            
            if (!Directory.Exists(saveImageInfo.Folder))
            {
                Directory.CreateDirectory(saveImageInfo.Folder);
            }            
            switch (format.Name)
            {
                case "BMP":
                    saveImageInfo.FileName += ".bmp";                                        
                    break;
                case "JPEG":
                    saveImageInfo.FileName += ".jpg";                    
                    break;
                case "PNG":
                    saveImageInfo.FileName += ".png";                    
                    break;
            }
            image.Save($"{saveImageInfo.Folder}{saveImageInfo.FileName}");
        }

    }
}

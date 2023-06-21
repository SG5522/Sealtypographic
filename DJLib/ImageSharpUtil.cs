using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System;
using System.IO;
using SixLabors.ImageSharp.Processing;
using System.Threading.Tasks;
using DJLib.Models;

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
        //public static string ImageToBase64(Image image,IImageFormat format)
        public static string ToBase64(ImageInfo imageInfo)
        {                    
            return imageInfo.ToBase64();
        }

        /// <summary>
        /// 轉成Stream
        /// </summary>
        /// <param name="ImageInfo">影像</param>
        public static Stream ToStream(ImageInfo imageInfo)
        {
            try
            {
                Stream stream = new MemoryStream();
                imageInfo.SourceImage.Save(stream, imageInfo.ImageFormat);
                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 檔案路徑圖片轉base64
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string PathImageFileToBase64(string fullPath)
        {                       
            return ToBase64(ImageInfo.FromPath(fullPath));
        }


        /// <summary>
        /// 存檔 支援格式(jpeg, bmp, gif, pbm, png, tga, tiff, Tga,WebP)
        /// </summary>
        /// <param name="image">影像</param>
        /// <param name="format">格式</param>
        /// <param name="saveFullPath">存檔資訊</param>
        public static void SaveFile(Image image, IImageFormat format, SaveFullPath saveFullPath)
        {            
            try
            {
                if (!Directory.Exists(saveFullPath.Folder))
                {
                    Directory.CreateDirectory(saveFullPath.Folder);
                }
                saveFullPath.FileName += $".{format.Name.ToLower()}";
                image.Save(Path.Combine(saveFullPath.Folder, saveFullPath.FileName));
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        /// <summary>
        /// 存檔 支援格式(jpeg, bmp, gif, pbm, png, tga, tiff, Tga,WebP)
        /// </summary>
        /// <param name="image">影像</param>
        /// <param name="format">格式</param>
        /// <param name="saveFullPath">存檔資訊</param>
        public static async Task SaveFileAsync(Image image, IImageFormat format, SaveFullPath saveFullPath)
        {
            try
            {
                if (!Directory.Exists(saveFullPath.Folder))
                {
                    Directory.CreateDirectory(saveFullPath.Folder);
                }
                saveFullPath.FileName += $".{format.Name.ToLower()}";
                await image.SaveAsync(Path.Combine(saveFullPath.Folder, saveFullPath.FileName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

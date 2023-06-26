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
        /// 檔案路徑圖片轉base64
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string PathImageFileToBase64(string fullPath)
        {                       
            return ImageInfo.FromPath(fullPath).ToBase64();
        }

        /// <summary>
        /// 存檔 支援格式(jpeg, bmp, gif, pbm, png, tga, tiff, Tga,WebP)
        /// </summary>
        /// <param name="image">影像</param>
        
        /// <param name="savePath">存檔路徑</param>
        public static void SaveFile(ImageInfo imageInfo, string savePath)
        {            
            try
            {
                string rootPath = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }
                imageInfo.SourceImage.Save(savePath);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        /// <summary>
        /// 存檔 支援格式(jpeg, bmp, gif, pbm, png, tga, tiff, Tga,WebP)
        /// </summary>        
        /// <param name="imageInfo">圖片資訊</param>
        /// <param name="savePath">存檔位置</param>
        public static async Task SaveFileAsync(ImageInfo imageInfo, string savePath)
        {
            try
            {
                string rootPath = Path.GetDirectoryName(savePath);                
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }                

                await imageInfo.SourceImage.SaveAsync(savePath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

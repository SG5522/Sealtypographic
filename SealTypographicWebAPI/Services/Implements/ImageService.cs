using DJLib;
using DJLib.Models;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services
{

    /// <summary>
    /// 取得圖像資料
    /// </summary>
    public class ImageService
    {
        private readonly SealPathOption sealConfig;

        /// <summary>
        /// 注入appsetting的ScanConfigPath資料
        /// </summary>
        /// <param name="options"></param>

        public ImageService(IOptionsSnapshot<SealPathOption> options)
        {
            sealConfig = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="fullpath"></param>        
        /// <returns></returns>
        public string GetPathToBase64(string fullpath)
        {            
            return ImageSharpUtil.PathImageFileToBase64(fullpath);            
        }

        /// <summary>
        /// Base64轉圖檔並存檔回傳存檔路徑
        /// </summary>
        /// <param name="imageBase64Info">ImageBase64資訊</param>
        /// <param name="IsResize">是否縮放</param>        
        public string GetImageBase64FullPath(ImageBase64Info imageBase64Info, bool IsResize)
        {
            string folderPath = GetImageFolder(imageBase64Info.SealType);            
            string dateFolder = Path.Combine
                                (
                                    imageBase64Info.CreateTime.Year.ToString(),
                                    imageBase64Info.CreateTime.Month.ToString(), 
                                    imageBase64Info.CreateTime.Day.ToString()
                                );
            SaveFullPath saveImageInfo = new()
            {                
                Folder = Path.Combine(folderPath,dateFolder)
            };

            ImageInfo imageInfo = ImageSharpUtil.Base64ToImageInfo(imageBase64Info.ImageBase64);
            if (!IsResize)
            {
                saveImageInfo.FileName = $"{imageBase64Info.Code}{imageBase64Info.CreateTime:yyyyMMHHmmssffff}";                
            }
            else
            {
                saveImageInfo.FileName = $"{imageBase64Info.Code}{"Thumbnail"}{imageBase64Info.CreateTime:yyyyMMHHmmssffff}";                
                ImageSharpUtil.ReSize(imageInfo.Image, sealConfig.ResizeScale);
            }
            ImageSharpUtil.SaveFile(imageInfo.Image, imageInfo.ImageFormat, saveImageInfo);            

            return Path.Combine(saveImageInfo.Folder, saveImageInfo.FileName);                        
        }

        /// <summary>
        /// 取得圖檔資料夾路徑(依類別)
        /// </summary>
        /// <param name="sealType"></param>
        /// <returns></returns>
        private string GetImageFolder(SealType sealType)
        {
            string folderPath;
            switch (sealType)
            {
                case SealType.Customer:
                    folderPath = sealConfig.Customer;
                    break;
                case SealType.Accountant:
                    folderPath = sealConfig.Accountant;
                    break;
                case SealType.Letterhead:
                    folderPath = sealConfig.Letterhead;
                    break;
                default :
                    folderPath = string.Empty;
                    break;
            }
            return folderPath;
        }        
    }
}

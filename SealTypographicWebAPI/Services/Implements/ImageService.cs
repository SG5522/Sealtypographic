using DJLib;
using DJLib.Models;
using Microsoft.Extensions.Options;
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
        private readonly SealConfigPath sealConfig;

        /// <summary>
        /// 注入appsetting的ScanConfigPath資料
        /// </summary>
        /// <param name="options"></param>

        public ImageService(IOptionsSnapshot<SealConfigPath> options)
        {
            sealConfig = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="path"></param>
        /// <param name="sealType"></param>
        /// <returns></returns>
        public string GetPathToBase64(string path,SealType sealType)
        {
            string folderPath = GetImageFolder(sealType);
            return ImageSharpUtil.PathImageFileToBase64($"{folderPath}{path}");            
        }
        
        /// <summary>
        /// Base64轉圖檔並存檔
        /// </summary>
        /// <param name="imageBase64Info">ImageBase64資訊</param>
        /// <param name="count"></param>
        public string SaveBase64ToFile(ImageBase64Info imageBase64Info, int count)
        {
            string folderPath = GetImageFolder(imageBase64Info.SealType);            
            string dateFolder = $"{imageBase64Info.CreateTime.Year}/{imageBase64Info.CreateTime.Month}/{imageBase64Info.CreateTime.Day}/" ;
            SaveImageInfo saveImageInfo = new()
            {
                Filename = $"{imageBase64Info.Code}{imageBase64Info.CreateTime:yyyyMMHHmmss}{count}",
                Folder = $"{folderPath}{dateFolder}"
            };

            ImageSharpUtil.Base64ToSaveImage(imageBase64Info.ImageBase64, saveImageInfo);

            return $"{dateFolder}{saveImageInfo.Filename}";            
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
                    folderPath = $"{sealConfig.SealRootPath}{sealConfig.Customer}";
                    break;
                case SealType.Accountant:
                    folderPath = $"{sealConfig.SealRootPath}{sealConfig.Accountant}";
                    break;
                case SealType.Letterhead:
                    folderPath = $"{sealConfig.SealRootPath}{sealConfig.Letterhead}";
                    break;
                default :
                    folderPath = string.Empty;
                    break;
            }
            return folderPath;
        }        
    }
}

using DJLib;
using DJLib.Models;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using DBEntities.Consts;
using SealTypographicWebAPI.Models;
using SixLabors.ImageSharp;

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
        /// <param name="isResize">是否縮放</param>        
        public string GetSavedImageFilePath(ImageBase64Info imageBase64Info, bool isResize)
        {
            SaveFullPath saveImageInfo = new()
            {                
                Folder = imageBase64Info.RootFolder()                
            };
                 
            ImageInfo imageInfo = ImageInfo.FromImageBase64(imageBase64Info.ImageBase64);
            if (!isResize)
            {
                saveImageInfo.FileName = imageBase64Info.ReName();
            }
            else
            {
                saveImageInfo.FileName = imageBase64Info.ReNameForThumbnail();
                ImageInfo.ReSize(imageInfo, sealConfig.ResizeScale);                
            }
            ImageSharpUtil.SaveFile(imageInfo.Image, imageInfo.ImageFormat, saveImageInfo);            

            return Path.Combine(saveImageInfo.Folder, saveImageInfo.FileName);                        
        }

        private void SavedImage()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageBase64Info"></param>
        /// <returns></returns>
        public string GetSavedImageFilePath(ImageBase64Info imageBase64Info)
        {            
            SaveFullPath saveImageInfo = new()
            {
                Folder = imageBase64Info.RootFolder()
            };

            ImageInfo imageInfo = ImageInfo.FromImageBase64(imageBase64Info.ImageBase64);
            saveImageInfo.FileName = imageBase64Info.ReNameForThumbnail();

            ImageSharpUtil.SaveFile(imageInfo.Image, imageInfo.ImageFormat, saveImageInfo);

            return Path.Combine(saveImageInfo.Folder, saveImageInfo.FileName);
        }        
    }
}

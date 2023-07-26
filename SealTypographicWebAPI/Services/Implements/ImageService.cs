using DJLib;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models;
using DBEntities.Consts;
using DJLib.Models;

namespace SealTypographicWebAPI.Services.Implements
{

    /// <summary>
    /// 取得圖像資料
    /// </summary>
    public class ImageService
    {
        private readonly SealPathOption sealPathOption;
        private readonly TemplateImagePathOption templateImagePathOption;

        /// <summary>
        /// 注入appsetting的ScanConfigPath資料
        /// </summary>
        /// <param name="sealPathOption"></param>
        /// <param name="templateImagePathOption"></param>

        public ImageService(IOptionsSnapshot<SealPathOption> sealPathOption, IOptionsSnapshot<TemplateImagePathOption> templateImagePathOption)
        {
            this.sealPathOption = sealPathOption.Value;
            this.templateImagePathOption = templateImagePathOption.Value;
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
        /// Base64轉圖檔並存檔回傳存檔路徑(存檔路徑透過ImageBase64Info生成)
        /// </summary>
        /// <param name="imageBase64Info">ImageBase64資訊</param>     
        public async Task<string> GetSavedImageFilePath(ImageBase64Info imageBase64Info)
        {
            string savePath = Path.Combine(imageBase64Info.RootFolder(), imageBase64Info.ReName());
            return await SaveImageAsync(imageBase64Info.ImageBase64, savePath, false);
        }

        /// <summary>
        /// Base64儲存縮圖並存檔回傳存檔路徑(存檔路徑透過ImageBase64Info生成)
        /// </summary>
        /// <param name="imageBase64Info"></param>
        /// <param name="isResize"></param>
        /// <returns></returns>
        public async Task<string> GetSavedImageThumbnailFilePath(ImageBase64Info imageBase64Info, bool isResize)
        {
            string savePath = Path.Combine(imageBase64Info.RootFolder(), imageBase64Info.ReNameForThumbnail());
            return await SaveImageAsync(imageBase64Info.ImageBase64, savePath, isResize);            
        }

        /// <summary>
        /// Base64轉圖檔並存檔
        /// saveFullPath為完整路徑
        /// </summary>
        /// <param name="imageBase64">base64圖檔</param>
        /// <param name="savePath">存檔路徑</param>
        /// <param name="isResize">是否縮放</param>
        /// <returns></returns>
        public void SaveImage(string imageBase64, string savePath, bool isResize)
        {
            ImageInfo imageInfo = ImageInfo.FromImageBase64(imageBase64);
            savePath += $".{imageInfo.ImageFormat.Name.ToLower()}";
            if (isResize)
            {
                imageInfo.ReSize(imageInfo, sealPathOption.ResizeScale);
            }
            ImageSharpUtil.SaveFile(imageInfo, savePath);
        }

        /// <summary>
        /// 非同步方式
        /// Base64轉圖檔並存檔
        /// savePath為完整路徑
        /// </summary>
        /// <param name="imageBase64">base64圖檔</param>
        /// <param name="savePath">存檔路徑</param>
        /// <param name="isResize">是否縮放</param>
        /// <returns></returns>
        public async Task<string> SaveImageAsync(string imageBase64, string savePath, bool isResize)
        {
            ImageInfo imageInfo = ImageInfo.FromImageBase64(imageBase64);
            savePath += $".{imageInfo.ImageFormat.Name.ToLower()}";            
            if (isResize)
            {
                imageInfo.ReSize(imageInfo, sealPathOption.ResizeScale);
            }
            await ImageSharpUtil.SaveFileAsync(imageInfo, savePath);
            return savePath;
        }

        /// <summary>
        /// 設定ImageBase64Info(依SealType設定路徑)
        /// </summary>
        /// <param name="code">編碼(檔名結構之一)</param>
        /// <param name="sealType">依Type決定SaveRootPath</param>
        /// <returns></returns>
        public ImageBase64Info SetImageBase64InfoWithSeal(string code, SealType sealType)
        {
            ImageBase64Info imageBase64Info = new()
            {
                Code = code,
            };
            switch (sealType)
            {
                case SealType.Customer:
                    imageBase64Info.SaveRootPath = sealPathOption.Customer;
                    break;
                case SealType.Accountant:
                    imageBase64Info.SaveRootPath = sealPathOption.Accountant;
                    break;
                case SealType.Letterhead:
                    imageBase64Info.SaveRootPath = sealPathOption.Letterhead;
                    break;
                case SealType.TemporarySeal:
                    imageBase64Info.SaveRootPath = sealPathOption.TemporarySeal;
                    break;
            }
            return imageBase64Info;
        }

        /// <summary>
        /// 設定ImageBase64Info(依SealType設定路徑)
        /// </summary>
        /// <param name="code">編碼(檔名結構之一)</param>
        /// <param name="sealType">依Type決定SaveRootPath</param>
        /// <returns></returns>
        public ImageBase64Info SetImageBase64InfoWithTemplate(string code, SealType sealType)
        {
            ImageBase64Info imageBase64Info = new()
            {
                Code = code,
            };
            switch (sealType)
            {
                case SealType.Customer:
                    imageBase64Info.SaveRootPath = templateImagePathOption.Customer;
                    break;
                case SealType.Accountant:
                    imageBase64Info.SaveRootPath = templateImagePathOption.Accountant;
                    break;
                case SealType.Letterhead:
                    imageBase64Info.SaveRootPath = templateImagePathOption.Letterhead;
                    break;
            }
            return imageBase64Info;
        }
    }
}

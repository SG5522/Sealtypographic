using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models;
using DBEntities.Consts;
using SixLabors.ImageSharp;
using DJImageLib.Models;
using CommonLib.Utils;
using DJImageLib.Utils;
using DJImageLib.Extensions;
using System.Buffers.Text;
using CommonLib.Extensions;

namespace SealTypographicWebAPI.Services.Implements
{

    /// <summary>
    /// 取得圖像資料
    /// </summary>
    public class ImageService
    {
        private SealPathOption sealPathOption;
        private TemplateImagePathOption templateImagePathOption;

        /// <summary>
        /// 注入appsetting的ScanConfigPath資料
        /// </summary>
        /// <param name="sealPathOption"></param>
        /// <param name="templateImagePathOption"></param>

        public ImageService(IOptionsMonitor<SealPathOption> sealPathOption, IOptionsMonitor<TemplateImagePathOption> templateImagePathOption)
        {
            this.sealPathOption = sealPathOption.CurrentValue;
            this.templateImagePathOption = templateImagePathOption.CurrentValue;

            sealPathOption.OnChange(options =>
            {
                this.sealPathOption = options;
            });

            templateImagePathOption.OnChange(options =>
            {
                this.templateImagePathOption = options;
            });
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="fullpath"></param>        
        /// <returns></returns>
        public string GetPathToBase64(string fullpath)
        {
            //return ImageSharpUtil.PathImageFileToBase64(fullpath);
            ImageModel imageModel = new();
            if(Path.GetExtension(fullpath) == "txt")
            {
                imageModel.Base64 = File.ReadAllText(fullpath);
            }
            else
            {
                imageModel.Base64 = Convert.ToBase64String(File.ReadAllBytes(fullpath));
            }                        

            return imageModel.DataUrl!;
        }

        /// <summary>
        /// Base64轉圖檔並存檔回傳存檔路徑(存檔路徑透過ImageBase64Info生成)
        /// </summary>
        /// <param name="imageBase64Info">ImageBase64資訊</param>     
        public async Task<string> GetSavedImageFilePath(ImageSaveInfo imageBase64Info)
        {
            string savePath = imageBase64Info.GetImageFilePath();
            return await SaveImageAsync(imageBase64Info.ImageBase64, savePath);
        }

        /// <summary>
        /// Base64儲存縮圖並存檔回傳存檔路徑(存檔路徑透過ImageBase64Info生成)
        /// </summary>
        /// <param name="imageBase64Info"></param>
        /// <param name="isResize"></param>
        /// <returns></returns>
        public async Task<string> GetSavedImageThumbnailFilePath(ImageSaveInfo imageBase64Info, bool isResize)
        {
            string result;
            string savePath = imageBase64Info.GetImageThumbnailFilePath();
            if(isResize)
            {
                result = await SaveImageAsync(imageBase64Info.ImageBase64, savePath, sealPathOption.ResizeScale);
            }
            else
            {
                result = await SaveImageAsync(imageBase64Info.ImageBase64, savePath);
            }

            return result;            
        }

        /// <summary>
        /// Base64轉圖檔並存檔
        /// saveFullPath為完整路徑
        /// </summary>
        /// <param name="imageBase64">base64圖檔</param>
        /// <param name="savePath">存檔路徑</param>
        /// <param name="resizeScale"></param>        
        /// <returns></returns>
        public static void SaveImage(string imageBase64, string savePath, float resizeScale = 0)
        {
            ImageModel imageModel = new() { DataUrl = imageBase64 };
            string savefullPath = Path.Combine(savePath, imageModel.ImageFormat!.Name.ToLower()!);
            if (resizeScale != 0)
            {               
                 ImageUtil.ReSizeBase64Only(imageBase64, resizeScale, resizeScale);
            }
            
            imageBase64.ToBytes().Save(savefullPath);                        
        }

        /// <summary>
        /// 非同步方式
        /// Base64轉圖檔並存檔
        /// savePath為完整路徑
        /// </summary>
        /// <param name="imageBase64">base64圖檔</param>
        /// <param name="savePath">存檔路徑</param>
        /// <param name="resizeScale">縮放參數</param>        
        /// <returns></returns>
        public async static Task<string> SaveImageAsync(string imageBase64, string savePath, float resizeScale = 0)
        {     
            ImageModel imageModel = new() { DataUrl = imageBase64 };
            
            if(imageModel.Base64 != null)
            {
                savePath = $"{savePath}.{imageModel.ImageFormat!.Name.ToLower()}";
                if (resizeScale != 0)
                {
                    imageModel.Base64 = ImageUtil.ReSizeBase64Only(imageModel.Base64, resizeScale, resizeScale);
                }                
                //儲存圖片
                await imageModel.Base64.ToBytes().SaveAsync(savePath);
            }
            else
            {
                throw new ArgumentException("Invalid image base64 string, unable to generate image data", nameof(imageBase64));
            }

            return savePath;
        }

        /// <summary>
        /// 設定ImageBase64Info(依SealType設定路徑)
        /// </summary>
        /// <param name="code">編碼(檔名結構之一)</param>
        /// <param name="sealType">依Type決定SaveRootPath</param>
        /// <returns></returns>
        public ImageSaveInfo SetImageBase64InfoWithSeal(string code, SealType sealType)
        {
            ImageSaveInfo imageBase64Info = new()
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
        public ImageSaveInfo SetImageBase64InfoWithTemplate(string code, SealType sealType)
        {
            ImageSaveInfo imageBase64Info = new()
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

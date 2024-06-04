using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models;
using DBEntities.Consts;
using SixLabors.ImageSharp;
using DJImageLib.Models;
using CommonLib.Utils;
using DJImageLib.Utils;
using DJImageLib.Extensions;
using CommonLib.Extensions;
using DBEntities;

namespace SealTypographicWebAPI.Services.Implements
{

    /// <summary>
    /// 取得圖像資料
    /// </summary>
    public class ImageService
    {
        private SealPathOption sealPathOption;
        private TemplateImagePathOption templateImagePathOption;
        private readonly SealTypographicDbContext dbContext;



        /// <summary>
        /// 注入appsetting的ScanConfigPath資料
        /// </summary>
        /// <param name="sealPathOption"></param>
        /// <param name="templateImagePathOption"></param>
        /// <param name="dbContext"></param>

        public ImageService(IOptionsMonitor<SealPathOption> sealPathOption,
                            IOptionsMonitor<TemplateImagePathOption> templateImagePathOption,
                            SealTypographicDbContext dbContext)
        {
            this.sealPathOption = sealPathOption.CurrentValue;
            this.templateImagePathOption = templateImagePathOption.CurrentValue;
            this.dbContext = dbContext;

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
            if (Path.GetExtension(fullpath) == "txt")
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
            if (isResize)
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
            string savefullPath = $"{savePath}.{imageModel.ImageFormat!.Name.ToLower()}";
            if (resizeScale != 0)
            {
                ImageUtil.ReSizeBase64Only(imageBase64, resizeScale, resizeScale);
            }

            imageBase64.ToBytes().Save(savefullPath);
        }

        /// <summary>
        /// 加密圖片並儲存
        /// </summary>
        /// <param name="imageBase64">Base64圖片字串</param>
        /// <param name="savePath">存檔路徑</param>
        /// <param name="rasKey">RAS公私鑰</param>        
        /// <param name="resizeScale">縮放大小</param>
        /// <returns>回傳加密後的Key</returns>
        public string EncryptImageWithKey(string imageBase64, string savePath, RASKey rasKey, float resizeScale = 0)
        {
            string result;

            ImageModel imageModel = new() { DataUrl = imageBase64 };

            if (imageModel.Base64 != null)
            {
                if (resizeScale != 0)
                {
                    ImageUtil.ReSizeBase64Only(imageModel.Base64, resizeScale, resizeScale);
                }

                result = CryptoUtil.Encrypt(imageModel.Base64, savePath, rasKey.PrivateKeyBase64, rasKey.PublicKeyBase64);
            }
            else
            {
                throw new ArgumentException("Invalid image base64 string, unable to generate image data", nameof(imageBase64));
            }

            return result;
        }


        /// <summary>
        /// 加密圖片並儲存(非同步)
        /// </summary>
        /// <param name="imageSaveInfo">ImageBase64資訊</param>
        /// <param name="rasKey">RAS公私鑰</param>        
        /// <param name="resizeScale">縮放大小</param>
        public async Task<string> EncryptImageWithKeyAsync(ImageSaveInfo imageSaveInfo, RASKey rasKey, float resizeScale = 0)
        {
            string result;

            ImageModel imageModel = new() { DataUrl = imageSaveInfo.ImageBase64 };            

            if (imageModel.Base64 != null)
            {
                if (resizeScale != 0)
                {
                    ImageUtil.ReSizeBase64Only(imageModel.Base64, resizeScale, resizeScale);
                }

                result = await CryptoUtil.EncryptAsync(imageModel.Base64, imageSaveInfo.GetImageFilePath(), rasKey.PrivateKeyBase64, rasKey.PublicKeyBase64);
            }
            else
            {
                throw new ArgumentException("Invalid image base64 string, unable to generate image data", nameof(imageSaveInfo.ImageBase64));
            }

            return result;
        }

        /// <summary>
        /// 解密圖案
        /// </summary>
        /// <param name="savePath">存檔路徑</param>
        /// <param name="encryptKey">加密後的key</param>
        /// <param name="rasKey">RAS公私鑰</param>
        /// <returns></returns>
        public string DecryptImage(string savePath, string encryptKey, RASKey rasKey)
            => CryptoUtil.Decrypt(savePath, encryptKey, rasKey.PrivateKeyBase64, rasKey.PublicKeyBase64);

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

            if (imageModel.Base64 != null)
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

        /// <summary>
        /// 取得RasKey
        /// </summary>
        /// <param name="userId">使用者Id</param>        
        /// <returns>回傳RasKey</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public RASKey GetRasKey(int userId) => GetRASKey(userId);

        /// <summary>
        /// 取得RasKey
        /// </summary>
        /// <param name="companyId"></param>        
        /// <returns>回傳RasKey</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public RASKey GetRasKeyFromCompany(int companyId) => GetRASKey(0, companyId);


        /// <summary>
        /// 取得RasKey
        /// </summary>
        /// <param name="userId">使用者Id</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>回傳RasKey</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private RASKey GetRASKey(int userId = 0, int companyId = 1)
        {
            RASKey? rasKey = dbContext.Companys
                            .Where
                            (
                                x => userId == 0 ?
                                x.Id == companyId : x.ApplicationUsers.Any(x => x.Id == userId)
                            )
                            .Select(x => new RASKey
                            {
                                PrivateKeyBase64 = File.ReadAllText(x.PrivateKeyFilePath),
                                PublicKeyBase64 = x.PublicKeyBase64
                            }).FirstOrDefault();

            if (rasKey == null || string.IsNullOrEmpty(rasKey.PublicKeyBase64) || string.IsNullOrEmpty(rasKey.PrivateKeyBase64))
            {
                throw new InvalidOperationException("No RSA key found for the specified company.");
            }

            return rasKey;
        }
    }
}

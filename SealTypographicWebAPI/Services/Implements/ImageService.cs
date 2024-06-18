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
using DBEntities.Entities.TypographicModels;
using DBEntities.Utils;
using SealTypographicWebAPI.Models.BaseModels;
using DBEntities.Entities.TemplateModels;

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
        /// <param name="imageSaveInfo">ImageBase64資訊</param>
        /// <param name="isThumbnail"></param>     
        public async Task SavedImageAsync(ImageSaveInfo imageSaveInfo, bool isThumbnail = false)
        {            
            imageSaveInfo.ReNamePath();
            
            if (!string.IsNullOrWhiteSpace(imageSaveInfo.ImageModel.Base64))
            {
                imageSaveInfo.FullPath = $"{imageSaveInfo.FullPath}.{imageSaveInfo.ImageModel.ImageFormat?.Name.ToLower()}";
                //儲存圖片
                await imageSaveInfo.ImageModel.Base64.ToBytes().SaveAsync(imageSaveInfo.FullPath);
                //確認縮圖處理
                if (isThumbnail)
                {
                    imageSaveInfo.ThumbnailScale = !string.IsNullOrWhiteSpace(imageSaveInfo.ThumbnailImageBase64) ? sealPathOption.ResizeScale : 0;
                    imageSaveInfo.ThumbnailFullPath = $"{imageSaveInfo.ThumbnailFullPath}.{imageSaveInfo.ImageModel.ImageFormat?.Name.ToLower()}";
                    //儲存圖片
                    await imageSaveInfo.ThumbnailImageBase64.ToBytes().SaveAsync(imageSaveInfo.ThumbnailFullPath);
                }                                
            }
            else
            {
                throw new ArgumentException("Invalid image base64 string, unable to generate image data", nameof(imageSaveInfo));
            }                        
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
        /// 
        /// </summary>
        /// <param name="imageSaveInfo"></param>
        /// <param name="isThumbnail"></param>
        private void SaveImage(ImageSaveInfo imageSaveInfo, bool isThumbnail = false)
        {
            imageSaveInfo.ReNamePath();

            if (!string.IsNullOrWhiteSpace(imageSaveInfo.ImageModel.Base64))
            {
                imageSaveInfo.FullPath = $"{imageSaveInfo.FullPath}.{imageSaveInfo.ImageModel.ImageFormat?.Name.ToLower()}";
                //儲存圖片
                imageSaveInfo.ImageModel.Base64.ToBytes().Save(imageSaveInfo.FullPath);
                //確認縮圖處理
                if (isThumbnail)
                {
                    imageSaveInfo.ThumbnailScale = !string.IsNullOrWhiteSpace(imageSaveInfo.ThumbnailImageBase64) ? sealPathOption.ResizeScale : 0;
                    imageSaveInfo.ThumbnailFullPath = $"{imageSaveInfo.ThumbnailFullPath}.{imageSaveInfo.ImageModel.ImageFormat?.Name.ToLower()}";
                    //儲存圖片
                    imageSaveInfo.ThumbnailImageBase64.ToBytes().Save(imageSaveInfo.ThumbnailFullPath);
                }
            }
            else
            {
                throw new ArgumentException("Invalid imageSaveInfo.ImageModel.Base64, unable to generate image data", nameof(imageSaveInfo));
            }
        }

        /// <summary>
        /// 加密圖片並儲存
        /// </summary>
        /// <param name="imageSaveInfo">Image存檔資訊</param>        
        /// <param name="isThumbnail">是否縮放</param>        
        /// <returns>回傳加密後的Key</returns>
        /// <exception cref="ArgumentException">imageSaveInfo.ImageModel.Base64 無資料可能是Base64不合法</exception>
        public void EncryptImage(ImageSaveInfo imageSaveInfo, bool isThumbnail = false)
        {
            if (string.IsNullOrWhiteSpace(imageSaveInfo.ImageModel.Base64))
            {
                throw new ArgumentException("Invalid image base64 string, unable to generate image data", nameof(imageSaveInfo));
            }

            imageSaveInfo.EncryptKey = CryptoUtil.Encrypt(
                                       imageSaveInfo.ImageModel.Base64,
                                       imageSaveInfo.FullPath, 
                                       imageSaveInfo.RSAKey.PublicKeyBase64);

            if (isThumbnail)
            {
                imageSaveInfo.ThumbnailScale = sealPathOption.ResizeScale;
                imageSaveInfo.ThumbnailEncryptKey = CryptoUtil.Encrypt(
                                                    imageSaveInfo.ThumbnailImageBase64, 
                                                    imageSaveInfo.ThumbnailFullPath, 
                                                    imageSaveInfo.RSAKey.PublicKeyBase64);
            }
        }


        /// <summary>
        /// 加密圖片並儲存(非同步)
        /// </summary>
        /// <param name="imageSaveInfo">Image存檔資訊</param>        
        /// <param name="isThumbnail">是否縮放</param>
        /// <returns>回傳加密後的Key</returns>
        /// <exception cref="ArgumentException">imageSaveInfo.ImageModel.Base64 無資料可能是Base64不合法</exception>
        public async Task EncryptImageAsync(ImageSaveInfo imageSaveInfo, bool isThumbnail = false)
        {      
            if(string.IsNullOrWhiteSpace(imageSaveInfo.ImageModel.Base64))
            {
                throw new ArgumentException("Invalid ImageModel.Base64 string, unable to generate image data", nameof(imageSaveInfo));
            }

            imageSaveInfo.EncryptKey = await CryptoUtil.EncryptAsync(
                                             imageSaveInfo.ImageModel.Base64, 
                                             imageSaveInfo.FullPath, 
                                             imageSaveInfo.RSAKey.PublicKeyBase64);

            if (isThumbnail)
            {
                imageSaveInfo.ThumbnailScale = sealPathOption.ResizeScale;
                imageSaveInfo.ThumbnailEncryptKey = await CryptoUtil.EncryptAsync(
                                                          imageSaveInfo.ThumbnailImageBase64, 
                                                          imageSaveInfo.ThumbnailFullPath, 
                                                          imageSaveInfo.RSAKey.PublicKeyBase64);
            }
        }

        /// <summary>
        /// 加密圖片並儲存(非同步)
        /// </summary>
        /// <param name="imageSaveInfo">Image存檔資訊</param>                
        /// <returns>回傳加密後的Key</returns>
        /// <exception cref="ArgumentException">imageSaveInfo.ImageModel.Base64 無資料可能是Base64不合法</exception>
        public async Task EncryptFileAsync(ImageSaveInfo imageSaveInfo)
        {
            if (string.IsNullOrWhiteSpace(imageSaveInfo.Base64))
            {
                throw new ArgumentException("Invalid imageSaveInfo.Base64 string, unable to generate image data", nameof(imageSaveInfo));
            }

            imageSaveInfo.EncryptKey = await CryptoUtil.EncryptAsync(
                                             imageSaveInfo.Base64,
                                             imageSaveInfo.FullPath,
                                             imageSaveInfo.RSAKey.PublicKeyBase64);
        }


        /// <summary>
        /// 解密檔案
        /// </summary>
        /// <param name="savePath">存檔路徑</param>
        /// <param name="encryptKey">加密後的key</param>
        /// <param name="rasKey">RAS公私鑰</param>
        /// <returns></returns>
        public byte[] DecryptFileToBytes(string savePath, string encryptKey, RSAKey rasKey)
            => DecryptFile(savePath, encryptKey, rasKey).ToBytes();

        /// <summary>
        /// 解密檔案
        /// </summary>
        /// <param name="savePath">存檔路徑</param>
        /// <param name="encryptKey">加密後的key</param>
        /// <param name="rasKey">RAS公私鑰</param>
        /// <returns></returns>
        public string DecryptFile(string savePath, string encryptKey, RSAKey rasKey)
            => CryptoUtil.Decrypt(savePath, encryptKey, rasKey.PrivateKeyBase64);

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
                    imageBase64Info.RootPath = sealPathOption.Customer;
                    break;
                case SealType.Accountant:
                    imageBase64Info.RootPath = sealPathOption.Accountant;
                    break;
                case SealType.Letterhead:
                    imageBase64Info.RootPath = sealPathOption.Letterhead;
                    break;
                case SealType.TemporarySeal:
                    imageBase64Info.RootPath = sealPathOption.TemporarySeal;
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
                    imageBase64Info.RootPath = templateImagePathOption.Customer;
                    break;
                case SealType.Accountant:
                    imageBase64Info.RootPath = templateImagePathOption.Accountant;
                    break;
                case SealType.Letterhead:
                    imageBase64Info.RootPath = templateImagePathOption.Letterhead;
                    break;
            }
            return imageBase64Info;
        }

        /// <summary>
        /// 儲存背景圖片與縮圖
        /// </summary>
        /// <typeparam name="T">各項樣版基本資料與檔案</typeparam>
        /// <param name="template">資料庫的樣板</param>
        /// <param name="form">來源圖像與縮圖</param>
        /// <param name="imageSaveInfo">Image存檔資訊</param>
        /// <returns></returns>
        public async Task SaveTemplateImage<T>(Template template, T form, ImageSaveInfo imageSaveInfo) where T : BaseTemplateWithFile
        {
            //取得來源圖片與縮圖
            imageSaveInfo.ImageBase64 = form.ImageBase64;
            imageSaveInfo.ThumbnailImageBase64 = form.ImageBase64Thumbnail;
            //儲存圖片(包含縮圖)
            await SavedImageAsync(imageSaveInfo, true);
            //給予圖片存檔位置
            template.ImageViewFullPath = imageSaveInfo.FullPath;
            template.ThumbnailFullPath = imageSaveInfo.ThumbnailFullPath;
        }

        /// <summary>
        /// 新增印鑑、簽印、圖片資料
        /// </summary>
        /// <param name="formSeals">輸入</param>
        /// <param name="code">編號</param>                
        /// <param name="userId">使用者Id</param>
        /// <returns>回傳TypographicResource的List內容</returns>
        public async Task<List<TypographicResource>> NewTypographyResource<T>(List<T> formSeals, string code, int userId = 1) where T : BaseCreateSeal
        {
            List<TypographicResource> typographyResources = new();

            //設定存檔路徑
            ImageSaveInfo imageSaveInfo = SetImageBase64InfoWithSeal(code, formSeals.First().SealType);
            imageSaveInfo.RSAKey = GetRasKey(userId);            

            foreach (T formSeal in formSeals)
            {                
                TypographicResource typographyResource = new()
                {
                    SealType = formSeal.SealType,                    
                    SubSealType = formSeal.SubSealType,
                    Sequence = formSeal.CommonSequence
                };

                //設定儲存的ImageBase64                
                imageSaveInfo.ImageBase64 = formSeal.ImageBase64;

                //依時間重新命名檔案
                imageSaveInfo.ReNamePath();

                //ImageBase64加密處理並存到指定資料夾
                await EncryptImageAsync(imageSaveInfo, true);

                //將加密後的Key和路徑存儲到 TypographyResource
                typographyResource.ImageEncryptKey = imageSaveInfo.EncryptKey;
                typographyResource.ImageFullPath = imageSaveInfo.FullPath;
                typographyResource.ThumbnailEncryptKey = imageSaveInfo.ThumbnailEncryptKey;
                typographyResource.ThumbnailFullPath = imageSaveInfo.ThumbnailFullPath;

                InputUtil.Set(typographyResource, userId, true);
                typographyResources.Add(typographyResource);
            }
            return typographyResources;
        }

        /// <summary>
        /// 取得RasKey
        /// </summary>
        /// <param name="userId">使用者Id</param>        
        /// <returns>回傳RasKey</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public RSAKey GetRasKey(int userId) => GetRASKey(userId);

        /// <summary>
        /// 取得RasKey
        /// </summary>
        /// <param name="companyId"></param>        
        /// <returns>回傳RasKey</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public RSAKey GetRasKeyFromCompany(int companyId) => GetRASKey(0, companyId);


        /// <summary>
        /// 取得RasKey
        /// </summary>
        /// <param name="userId">使用者Id</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>回傳RasKey</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private RSAKey GetRASKey(int userId = 0, int companyId = 1)
        {
            RSAKey? rsaKey = dbContext.Companys
                            .Where
                            (
                                x => (userId == 0 || userId == 1) ?
                                x.Id == companyId : x.ApplicationUsers.Any(x => x.Id == userId)
                            )
                            .Select(x => new RSAKey
                            {
                                PrivateKeyBase64 = File.ReadAllText(x.PrivateKeyFilePath),
                                PublicKeyBase64 = x.PublicKeyBase64
                            }).FirstOrDefault();

            if (rsaKey == null || string.IsNullOrEmpty(rsaKey.PublicKeyBase64) || string.IsNullOrEmpty(rsaKey.PrivateKeyBase64))
            {
                throw new InvalidOperationException("No RSA key found for the specified company.");
            }

            return rsaKey;
        }
    }
}

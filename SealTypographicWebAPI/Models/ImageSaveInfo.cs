using DJImageLib.Models;
using DJImageLib.Utils;

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// Image存檔資訊
    /// </summary>
    public class ImageSaveInfo
    {
        private string imageBase64;

        private string rootPath;

        private float thumbnailScale;

        /// <summary>
        /// 編號
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Base64圖片字串
        /// </summary>
        public string ImageBase64
        {
            get => imageBase64;
            set
            {
                imageBase64 = value;
                if (!string.IsNullOrWhiteSpace(imageBase64))
                {
                    ImageModel = new() { DataUrl = imageBase64 };
                }
                else
                {                                        
                    if (ImageModel != null)
                    {
                        ImageModel.DataUrl = string.Empty;
                    }

                    ThumbnailImageBase64 = string.Empty;
                }
            }
        }

        /// <summary>
        /// Base64檔案(非圖片)
        /// </summary>
        public string Base64 { get; set; }

        /// <summary>
        /// 圖像加密Key
        /// </summary>
        public string EncryptKey { get; set; }

        /// <summary>
        /// 縮圖加密Key
        /// </summary>
        public string ThumbnailEncryptKey { get; set; }

        /// <summary>
        /// 縮圖比例
        /// </summary>
        public float ThumbnailScale { 
            get => thumbnailScale;
            set
            {
                thumbnailScale = value;
                if (thumbnailScale > 0 && !string.IsNullOrWhiteSpace(ImageModel.Base64))
                {
                    ThumbnailImageBase64 = ImageUtil.ReSizeBase64Only(ImageModel.Base64, thumbnailScale, thumbnailScale);
                }
                else
                {
                    ThumbnailImageBase64 = string.Empty;
                }
            }
        }

        /// <summary>
        /// Base64縮圖字串
        /// </summary>
        public string ThumbnailImageBase64 { get; set; }

        /// <summary>
        /// 圖片模組
        /// </summary>
        public ImageModel ImageModel { get; private set; }

        /// <summary>
        /// RSA 公私鑰
        /// </summary>
        public RSAKey RSAKey { get; set; }

        /// <summary>
        /// 存檔根目錄位置
        /// </summary>
        public string RootPath 
        {
            get => rootPath;
            set
            {
                rootPath = value;
                if (!string.IsNullOrWhiteSpace(rootPath))
                {
                    ReNamePath();
                }
            }
        }

        /// <summary>
        /// 存檔完整路徑
        /// </summary>
        public string FullPath { get; set; } = string.Empty;

        /// <summary>
        /// 存檔縮圖完整路徑
        /// </summary>
        public string ThumbnailFullPath { get; set; } = string.Empty;

        /// <summary>
        /// 建檔時間
        /// </summary>
        public DateTime CreateTime => DateTime.Now;

        /// <summary>
        /// 取得存檔路徑
        /// </summary>
        public string RootFolder()
        {
            return Path.Combine
                   (
                       rootPath,
                       CreateTime.Year.ToString(),
                       CreateTime.Month.ToString(),
                       CreateTime.Day.ToString()
                   );
        }

        /// <summary>
        /// 更改FullPath and ThumbnailFullPath 的檔名
        /// </summary>
        public void ReNamePath()
        {
            FullPath = GetFilePath();
            ThumbnailFullPath = GetThumbnailFilePath();
        }

        /// <summary>
        /// 取得重新命名檔名
        /// </summary>
        /// <returns></returns>
        public string GetFilePath() => Path.Combine(RootFolder(), GenerateFileName());

        /// <summary>
        /// 取得重新命名縮圖檔名
        /// </summary>
        /// <returns></returns>
        public string GetThumbnailFilePath() => Path.Combine(RootFolder(), GenerateThumbnailFileName());

        private string GenerateFileName() => $"{Code}{CreateTime:yyyyMMddHHmmssffff}";

        private string GenerateThumbnailFileName() => $"thumbnail{GenerateFileName()}";

    }    
}

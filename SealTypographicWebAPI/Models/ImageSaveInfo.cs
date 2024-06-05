using DJImageLib.Models;

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// Image存檔資訊
    /// </summary>
    public class ImageSaveInfo
    {
        private string imageBase64;

        private string rootPath;

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
            }
        }

        /// <summary>
        /// 圖片模組
        /// </summary>
        public ImageModel ImageModel { get; set; }

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
                    FullPath = GetFilePath();
                    ThumbnailFullPath = GetThumbnailFilePath();
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
        public string ReName() => $"{Code}{CreateTime:yyyyMMHHmmssffff}";

        /// <summary>
        /// 取得重新命名檔名
        /// </summary>
        /// <returns></returns>
        public string ReNameForThumbnail() => $"{"thumbnail"}{ReName()}";        

        /// <summary>
        /// 取得圖檔存檔路徑
        /// </summary>
        /// <returns></returns>
        public string GetFilePath() => Path.Combine(RootFolder(), ReName());

        /// <summary>
        /// 取得圖檔縮圖存檔路徑
        /// </summary>
        /// <returns></returns>
        public string GetThumbnailFilePath() => Path.Combine(RootFolder(), ReNameForThumbnail());
    }
}

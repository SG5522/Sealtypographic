using DBEntities.Consts;
using DJLib.Models;

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// ImageBase64資訊
    /// </summary>
    public class ImageBase64Info
    {
        /// <summary>
        /// 編號
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Base64圖片字串
        /// </summary>
        public string ImageBase64 { get; set; } = string.Empty;

        /// <summary>
        /// 存檔根目錄位置
        /// </summary>
        public string SaveRootPath { get; set; } = string.Empty;

        /// <summary>
        /// 建檔時間
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 取得存檔路徑
        /// </summary>
        public string RootFolder()
        {
            return Path.Combine
                    (
                        SaveRootPath,
                        CreateTime.Year.ToString(),
                        CreateTime.Month.ToString(),
                        CreateTime.Day.ToString()
                    );
        }

        /// <summary>
        /// 取得重新命名檔名
        /// </summary>
        /// <returns></returns>
        public string ReName()
        {
            return $"{Code}{CreateTime:yyyyMMHHmmssffff}";
        }

        /// <summary>
        /// 取得重新命名檔名
        /// </summary>
        /// <returns></returns>
        public string ReNameForThumbnail()
        {
            return $"{"thumbnail"}{Code}{CreateTime:yyyyMMHHmmssffff}";
        }
    }
}

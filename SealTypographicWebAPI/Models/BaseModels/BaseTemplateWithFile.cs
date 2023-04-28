using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 各項樣版基本資料與檔案
    /// </summary>
    public abstract class BaseTemplateWithFile : BaseTemplate
    {
        /// <summary>
        /// 背景圖片
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }

        /// <summary>
        /// 背景圖片
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64Thumbnail { get; set; }
    }
}

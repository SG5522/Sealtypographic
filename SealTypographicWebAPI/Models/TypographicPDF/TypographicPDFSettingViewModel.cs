using DJSpire.Models;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// PDF輸出前的一部份資料抓取
    /// </summary>
    public class TypographicPDFSettingViewModel : ResponseViewModel
    {
        /// <summary>
        /// 原始檔名
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { get; set; }

        /// <summary>
        /// 排版頁數
        /// </summary>
        public int EditPageCount { get; set; }

        /// <summary>
        /// 空白頁數
        /// </summary>
        public int BlankPageCount { get; set; }

        /// <summary>
        /// 預設輸出PDF檔名
        /// </summary>
        public string DefaultPdfFileName { get; set; }
    }
}

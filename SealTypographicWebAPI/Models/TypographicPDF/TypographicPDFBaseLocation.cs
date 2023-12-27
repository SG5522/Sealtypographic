using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// PDF上印鑑位置與影像編輯參數
    /// </summary>
    public abstract class TypographicPDFBaseLocation : BaseLocation
    {
        /// <summary>
        /// 影像處理後的ImageBase64
        /// </summary>
        public string? EditPdfImageBase64 { get; set; }
    }
}

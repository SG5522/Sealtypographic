using DJSpire.Consts;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 輸出PDF檔案時的設定
    /// </summary>
    public class TypographicPDFMakeSetting
    {
        /// <summary>
        /// PDFID
        /// </summary>
        public int TypographicPDFId {get; set; }

        /// <summary>
        /// PDF輸出顏色
        /// 0 彩色(原色)
        /// 1 黑白(灰階)
        /// </summary>
        public PDFColor PDFColor { get; set; }

        /// <summary>
        /// 是否輸出空白頁
        /// </summary>
        public bool IsBlank { get; set; }
    }
}

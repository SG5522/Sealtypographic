using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排板PDFPage搜尋
    /// </summary>
    public class TypographicPDFPageSearch
    {
        /// <summary>
        ///排版ID
        /// </summary>
        /// <example>1</example>
        [Required]
        public int PdfId { get; set; }

        /// <summary>
        /// PDF頁次
        /// </summary>        
        /// <example>1</example>
        public int PageNumber { get; set; }
    }
}

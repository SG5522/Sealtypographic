using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 
    /// </summary>
    public class TypographicPDFSearch
    {
        /// <summary>
        /// 客戶Id
        /// </summary>
        [MaxLength(6)]
        [Required]
        public string CustomerId { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        [Required]
        public string Quarter { get; set; }
    }
}

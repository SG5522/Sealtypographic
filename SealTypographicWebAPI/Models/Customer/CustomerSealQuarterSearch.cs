using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶印鑑搜尋(依客戶ID與季度ID)
    /// </summary>
    public class CustomerSealQuarterSearch
    {
        /// <summary>
        /// 客戶Id
        /// </summary>
        /// <example>1</example>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        /// <example>111年Q1</example>
        [Required]
        public string Quarter { get; set; }
    }
}

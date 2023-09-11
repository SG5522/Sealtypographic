using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶印鑑組
    /// </summary>
    public class CustomerSealForm
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        /// <example>1</example>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        /// <example>111Q1</example>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶印鑑
        /// </summary>
        public List<CustomerSeal> Seals { get; set; }
    }
}

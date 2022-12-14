using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 顯示一筆審核資料用的搜尋
    /// </summary>
    public class CustomerSealReviewQuarterSearch
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <example>0</example>        
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        /// <example>111年Q1</example>
        public string Quarter { get; set; }
    }
}

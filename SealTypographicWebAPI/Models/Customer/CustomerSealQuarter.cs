using SealTypographicWebAPI.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶季度搜尋
    /// </summary>
    public class CustomerSealQuarter
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

        /// <summary>
        /// 印鑑審查狀態
        /// </summary>
        public string ReviewStatusString { get; set; }

    }

    /// <summary>
    /// 客戶季度搜尋表
    /// </summary>
    public class CustomerSealQuarters : ResponseViewModel
    {
        /// <summary>
        /// 客戶季度搜尋表
        /// </summary>
        public List<CustomerSealQuarter> Quarters { get; set; }
    }
}

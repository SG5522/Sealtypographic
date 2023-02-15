using SealTypographicWebAPI.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶印鑑季度
    /// </summary>
    public class CustomerSealQuarterViewModel
    {
        /// <summary>
        /// 印鑑季度Id
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        /// <example>111Q1</example>
        public string? Quarter { get; set; }

        /// <summary>
        /// 印鑑審查狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }

    }

    /// <summary>
    /// 客戶印鑑季度列表
    /// </summary>
    public class CustomerSealQuarterResponse : ResponseViewModel
    {
        /// <summary>
        /// new CustomerSealQuarterView
        /// </summary>
        public CustomerSealQuarterResponse() 
        {
            CustomerSealQuarters = new();
        }

        /// <summary>
        /// 客戶季度搜尋表
        /// </summary>
        public List<CustomerSealQuarterViewModel> CustomerSealQuarters { get; set; }
    }
}

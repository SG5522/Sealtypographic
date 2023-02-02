using SealTypographicWebAPI.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶印鑑季度
    /// </summary>
    public class CustomerSealViewQuarter
    {
        /// <summary>
        /// 客戶Id
        /// </summary>
        /// <example>1</example>
        public int CustomerId { get; set; }

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
    public class CustomerSealQuarterViews : ResponseViewModel
    {
        /// <summary>
        /// new CustomerSealQuarterView
        /// </summary>
        public CustomerSealQuarterViews() 
        {
            Quarters = new();
        }

        /// <summary>
        /// 客戶季度搜尋表
        /// </summary>
        public List<CustomerSealViewQuarter> Quarters { get; set; }
    }
}

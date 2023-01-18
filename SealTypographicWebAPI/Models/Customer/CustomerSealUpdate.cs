using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 需要異動客戶印鑑資料
    /// </summary>
    public class CustomerSealUpdate
    {
        /// <summary>
        /// 客戶Id
        /// </summary>        
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
        /// 刪除客戶印鑑列表(ID)
        /// </summary>
        public List<int> DeleteCustomerSealIds { get; set; }

        /// <summary>
        /// 更新客戶印鑑列表
        /// </summary>
        public List<CustomerSealUpdateForm> UpdateCustomerSeals { get; set; }

        /// <summary>
        /// 新增客戶印鑑列表
        /// </summary>
        public List<CustomerSeal> CreateCustomerSeals { get; set; }
   }
}

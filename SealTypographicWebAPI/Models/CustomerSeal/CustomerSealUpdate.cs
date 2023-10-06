using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.CustomerSeal
{
    /// <summary>
    /// 需要異動客戶印鑑資料
    /// </summary>
    public class CustomerSealUpdate
    {
        /// <summary>
        /// 客戶印鑑季度Id
        /// </summary>        
        [Required]
        public int CustomerSealQuarterId { get; set; }

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

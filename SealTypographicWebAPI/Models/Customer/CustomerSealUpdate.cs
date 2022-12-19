using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 印鑑組資料(含ID)
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
        /// <example>111年Q1</example>
        [Required]
        public string Quarter { get; set; }

        /// <summary>
        /// 刪除印鑑列表(ID)
        /// </summary>
        public List<int> DeleteCustomerSealIds { get; set; }

        /// <summary>
        /// 更新印鑑列表
        /// </summary>
        public List<CustomerSealFormUpdate> UpdateCustomerSeals { get; set; }

        /// <summary>
        /// 新增印鑑列表
        /// </summary>
        public List<CustomerSealForm> CreateCustomerSeals { get; set; }
   }
}

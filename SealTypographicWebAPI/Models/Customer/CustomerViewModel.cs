using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶
    /// </summary>
    public class CustomerViewModel : BaseNameData
    {
        /// <summary>
        /// 統一編號 (Business administration number)
        /// </summary>
        public string BAN { get; set; }

        /// <summary>
        /// 客戶編號(更新或搜尋使用)
        /// </summary>
        /// <example>AAA001</example>
        [Required]        
        public string CustomerNumber { get; set; }
    }
}

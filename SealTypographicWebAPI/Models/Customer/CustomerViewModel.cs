using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶
    /// </summary>
    public class CustomerViewModel : CustomerViewBase
    {
        /// <summary>
        /// 顯示最新季度
        /// </summary>
        /// <example>111YQ1</example>        
        public string? Quarter { get; set; }
    }
}

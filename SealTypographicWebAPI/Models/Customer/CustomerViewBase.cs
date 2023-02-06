using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶
    /// </summary>
    public class CustomerViewBase : BaseData
    {
        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>映像公司</example>
        public string Name { get; set; }

        /// <summary>
        /// 統一編號 (Business administration number)
        /// </summary>
        public string BAN { get; set; }

        /// <summary>
        /// 客戶編號(更新或搜尋使用)
        /// </summary>
        /// <example>AAA001</example>
        [Required]        
        public string Code { get; set; }

    }
}

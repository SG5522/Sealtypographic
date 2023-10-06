using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶基本資料(精簡)
    /// </summary>
    public class CustomerSummary : BaseData
    {
        /// <summary>
        /// 客戶編號(更新或搜尋使用)
        /// </summary>
        /// <example>AAA001</example>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>映像公司</example>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 統一編號 (Business administration number)
        /// </summary>
        public string BAN { get; set; }
    }
}

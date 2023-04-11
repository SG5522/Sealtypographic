using System.ComponentModel.DataAnnotations;
using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶
    /// </summary>
    public class CustomerViewModel : CustomerSummary
    {
        /// <summary>
        /// 統一編號 (Business administration number)
        /// </summary>
        public string BAN { get; set; }

        /// <summary>
        /// 顯示最新季度Id
        /// </summary>        
        public int? CustomerSealQuarterId { get; set; }

        /// <summary>
        /// 有無草稿狀態
        /// </summary>
        public bool IsDraff { get; set; }

        /// <summary>
        /// 有無待審狀態
        /// </summary>
        public bool IsPending { get; set; }

        /// <summary>
        /// 有無退件狀態
        /// </summary>
        public bool IsReject { get; set; }

    }
}

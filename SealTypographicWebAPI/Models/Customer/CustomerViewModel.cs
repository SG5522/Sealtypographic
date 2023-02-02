using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Consts;
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
        /// <example>111Q1</example>        
        public string? Quarter { get; set; }

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

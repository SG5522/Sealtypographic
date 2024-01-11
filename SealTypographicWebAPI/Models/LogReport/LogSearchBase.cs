using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// Log基本搜尋
    /// </summary>
    public abstract class LogSearchBase : PaginateViewModel
    {
        /// <summary>
        /// 起始日期
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
    }
}

using SealTypographicWebAPI.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 分頁搜尋
    /// </summary>
    public class PaginateSearch
    {
        /// <summary>
        /// 現在頁數(不得小於0)
        /// </summary>
        /// <example>1</example>
        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        /// <summary>
        /// 每頁資料筆數(不得小於0)
        /// </summary>
        /// <example>5</example>
        [Required]
        [Range(1, (int)PageSizeLimit.Max)]
        public int PageSize { get; set; }
    }

}

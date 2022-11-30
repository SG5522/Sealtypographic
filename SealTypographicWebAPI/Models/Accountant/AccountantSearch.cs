using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師分頁搜尋
    /// </summary>
    public class AccountantSearch
    {
        /// <summary>
        /// 會計師ID或名字或是群組名稱
        /// </summary>        
        public string? IdOrNameOrGroupsName { get; set; }

        /// <summary>
        /// 狀態
        ///-1.全部
        /// 0.待審
        /// 1.通過(審核完成)
        /// 2.退件
        /// 3.隱藏(被刪除時的狀態)
        /// </summary>
        /// <example>0</example>
        [Required]
        [Range(-1, 3)]
        public int Status { get; set; }

        /// <summary>
        /// 現在頁次(不得小於0)
        /// </summary>
        /// <example>1</example>
        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        /// <summary>
        /// 單頁筆數(不得小於0)
        /// </summary>
        /// <example>5</example>
        [Required]
        [Range(1, PageSizeLimit.Max)]
        public int PageSize { get; set; }
    }
}

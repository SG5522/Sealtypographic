using SealTypographicWebAPI.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.PublicModel
{
    /// <summary>
    /// 分頁搜尋
    /// </summary>
    public class PaginateSearchWithReviewStatus : PaginateSearch
    {
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
        public ReviewStatus ReviewStatus { get; set; }
    }

}

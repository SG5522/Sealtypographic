using Spire.Pdf;
using System.Reflection.Metadata;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 各種分頁回傳結果
    /// </summary>
    public abstract class PaginateViewModel : ResponseViewModel
    {        
        /// <summary>
        /// 目前頁碼
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每頁資料筆數(不得小於0)
        /// </summary>
        /// <example>5</example>
        public int PageSize { get; set; }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage => (TotalCount == 0 && PageSize == 0) ? 0 : TotalCount / PageSize + (TotalCount % PageSize == 0 ? 0 : 1);
    }
}

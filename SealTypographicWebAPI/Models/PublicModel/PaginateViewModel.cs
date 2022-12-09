namespace SealTypographicWebAPI.Models.PublicModel
{
    /// <summary>
    /// 各種分頁回傳結果
    /// </summary>
    public class PaginateViewModel : ResponseViewModel
    {
        /// <summary>
        /// 現在頁數
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每頁資料筆數(不得小於0)
        /// </summary>
        /// <example>5</example>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public int TotalCount { get; set; }
    }
}

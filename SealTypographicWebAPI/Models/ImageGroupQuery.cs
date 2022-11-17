namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 圖片群組分頁搜尋
    /// </summary>
    public class ImageGroupQuery
    {
        /// <summary>
        /// 搜尋群組名稱或是群組類別
        /// </summary>
        /// <example>公司章 customer</example>
        public string? NameOrType { get; set; }

        /// <summary>
        /// 現在頁數(不得小於0)
        /// </summary>
        /// <example>1</example>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每頁資料筆數(不得小於0)
        /// </summary>
        /// <example>5</example>
        public int PageSize { get; set; }
    }
}

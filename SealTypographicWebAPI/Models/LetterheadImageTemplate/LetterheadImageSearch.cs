using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖片歷程分頁搜尋
    /// </summary>
    public class LetterheadImageSearch : PaginateSearch
    {
        /// <summary>
        /// 搜尋信頭Id
        /// </summary>
        /// <example>1</example>
        public int LetterheadId { get; set; }
    }
}

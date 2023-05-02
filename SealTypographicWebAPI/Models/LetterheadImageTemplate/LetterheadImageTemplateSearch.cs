using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LetterheadTemplate
{
    /// <summary>
    /// 信頭樣板分頁搜尋
    /// </summary>
    public class LetterheadImageTemplateSearch : PaginateSearch
    {
        /// <summary>
        /// 關鍵字搜尋 (樣板名稱)
        /// </summary>
        /// <example>預設樣板</example>
        public string? KeyWord { get; set; }
    }
}

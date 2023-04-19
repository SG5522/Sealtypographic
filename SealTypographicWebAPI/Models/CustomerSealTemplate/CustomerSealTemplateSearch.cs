using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealTemplate
{
    /// <summary>
    /// 客戶印鑑樣板分頁搜尋
    /// </summary>
    public class CustomerSealTemplateSearch : PaginateSearch
    {
        /// <summary>
        /// 關鍵字搜尋 (樣板名稱)
        /// </summary>
        /// <example>預設樣板</example>
        public string? KeyWord { get; set; }

    }
}

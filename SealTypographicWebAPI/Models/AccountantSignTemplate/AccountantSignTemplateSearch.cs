using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantSignTemplate
{
    /// <summary>
    /// 會計師簽印樣板分頁搜尋
    /// </summary>
    public class AccountantSignTemplateSearch : PaginateSearch
    {
        /// <summary>
        /// 關鍵字搜尋 (樣板名稱)
        /// </summary>
        /// <example>預設樣板</example>
        public string? KeyWord { get; set; }

    }
}

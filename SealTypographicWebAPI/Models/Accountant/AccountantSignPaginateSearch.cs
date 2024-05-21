using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSeal
{
    /// <summary>
    /// 會計師簽印分頁搜尋
    /// </summary>
    public class AccountantSignPaginateSearch : PaginateSearch
    {
        /// <summary>
        /// 客戶Id
        /// </summary>
        public int AccountId { get; set; }
    }
}

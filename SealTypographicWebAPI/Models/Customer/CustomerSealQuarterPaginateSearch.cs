using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 印鑑季度分頁搜尋
    /// </summary>
    public class CustomerSealQuarterPaginateSearch : PaginateSearch
    {
        /// <summary>
        /// 客戶Id
        /// </summary>
        public int CustomerId { get; set; }
    }
}

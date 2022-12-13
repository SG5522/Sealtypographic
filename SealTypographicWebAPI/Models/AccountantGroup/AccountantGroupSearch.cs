using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroup
{
    /// <summary>
    /// 會計師群組分頁搜尋
    /// </summary>
    public class AccountantGroupSearch : PaginateSearch
    {
        /// <summary>
        /// 會計師群組名稱
        /// </summary>
        public string? GroupName { get; set; }
    }
}

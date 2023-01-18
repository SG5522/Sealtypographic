using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroup
{
    /// <summary>
    /// 會計師群組搜尋條件(分頁)
    /// </summary>
    public class AccountantGroupSearch : PaginateSearch
    {
        /// <summary>
        /// 會計師群組名稱
        /// </summary>
        public string? GroupName { get; set; }
    }
}

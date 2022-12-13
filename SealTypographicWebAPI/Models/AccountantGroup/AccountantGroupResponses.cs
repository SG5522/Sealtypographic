using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroup
{
    /// <summary>
    /// 依搜尋結果顯示會計師列表
    /// </summary>
    public class AccountantGroupResponses : PaginateViewModel
    {
        /// <summary>
        /// 會計師群組列表
        /// </summary>
        public List<AccountantGroupViewModel> AccountantGroups { get; set; }
    }
}

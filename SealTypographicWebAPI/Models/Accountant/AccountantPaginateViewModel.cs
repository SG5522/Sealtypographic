using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 依搜尋結果顯示會計師列表
    /// </summary>
    public class AccountantPaginatesViewModel : PaginateViewModel
    {
        /// <summary>
        /// 會計師列表
        /// </summary>
        public List<AccountantViewModel> AccountantViewModels { get; set; }
    }
}

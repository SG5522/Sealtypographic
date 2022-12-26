using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師列表(含啟用時間)
    /// </summary>
    public class AccountantViewModelWithCreateDate : AccountantViewModel
    {
        /// <summary>
        /// 啟用時間
        /// </summary>
        public DateTime? GroupCreateDate { get; set; }
    }

    /// <summary>
    /// 依搜尋結果顯示會計師列表
    /// </summary>
    public class AccountantPaginatesViewModel : PaginateViewModel
    {
        /// <summary>
        /// 會計師列表
        /// </summary>
        public List<AccountantViewModelWithCreateDate> AccountantViewModels { get; set; }
    }
}

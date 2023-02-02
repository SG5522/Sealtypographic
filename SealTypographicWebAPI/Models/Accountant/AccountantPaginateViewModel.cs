using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師包含啟用時間列表
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
    public class AccountantPaginateViewModel : PaginateViewModel
    {
        /// <summary>
        /// new ViewModels
        /// </summary>
        public AccountantPaginateViewModel()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 會計師列表
        /// </summary>
        public List<AccountantViewModelWithCreateDate> ViewModels { get; set; }
    }
}

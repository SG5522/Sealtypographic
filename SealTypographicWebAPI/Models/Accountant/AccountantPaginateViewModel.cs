using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師包含啟用時間列表
    /// </summary>
    public class AccountantViewModelWithCreateDate : BaseName
    {
        /// <summary>
        /// 會計師編號
        /// </summary>
        /// <example>ACC001</example>
        public string AccountantNumber { get; set; }

        /// <summary>
        /// 會計師群組名稱
        /// </summary>
        /// <example>台北群組</example>
        public List<string> AccountantGroupName { get; set; }

        /// <summary>
        /// 啟用時間
        /// </summary>
        /// <example>1</example>
        public int AccountantSignGroupId { get; set; }
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

using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 非群組成員
    /// </summary>
    public class NotThisGroupMember : PaginateViewModel
    {
        /// <summary>
        /// 群組ID
        /// </summary>
        public int AccountantGroupid { get; set; }

        /// <summary>
        /// 非此群組成員
        /// </summary>
        public List<AccountantViewModel> AccountantViewModels { get; set; }
    }
}

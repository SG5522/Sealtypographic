using SealTypographicWebAPI.Models.Accountant;

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 非群組成員
    /// </summary>
    public class NotThisGroupMember : ResponseViewModel
    {
        /// <summary>
        /// 群組ID
        /// </summary>
        public string AccountantGroupid { get; set; }

        /// <summary>
        /// 非此群組成員
        /// </summary>
        public List<AccountantViewModel> AccountantViewModels { get; set; }
    }
}

using SealTypographicWebAPI.Models.PublicModel;

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 會計師群組成員資料
    /// </summary>
    public class AccountantGroupMember : BaseName
    {
        /// <summary>
        /// 會計師編號
        /// </summary>
        public string AccountantNumber { get; set; }
    }

    /// <summary>
    /// 會計師群組成員(多筆 回傳用)
    /// </summary>
    public class AccountantGroupMembers : PaginateViewModel
    {

        /// <summary>
        /// 成員
        /// </summary>
        public List<AccountantGroupMember> Members { get; set; }
    }
}

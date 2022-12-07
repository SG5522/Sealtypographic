namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 會計師群組成員資料
    /// </summary>
    public class AccountantGroupMember
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 會計師編號
        /// </summary>
        public string AccountantNumber { get; set; }

        /// <summary>
        /// 會計師名稱
        /// </summary>
        public string Name { get; set; }
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

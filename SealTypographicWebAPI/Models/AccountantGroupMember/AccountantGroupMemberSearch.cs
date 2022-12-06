namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 群組搜尋條件
    /// </summary>
    public class AccountantGroupMemberSearch : PaginateSearch
    {
        /// <summary>
        /// 群組Id
        /// </summary>
        public string AccountantGroupId { get; set; }
    }
}

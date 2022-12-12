namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 會計師群組成員資料
    /// </summary>
    public class AccountantGroupMemberForm
    {
        /// <summary>
        /// 群組ID
        /// </summary>
        public int AccountantGroupId { get; set; }

        /// <summary>
        /// 成員Id
        /// </summary>
        public List<int> AccountantIds { get; set; }
    }
}

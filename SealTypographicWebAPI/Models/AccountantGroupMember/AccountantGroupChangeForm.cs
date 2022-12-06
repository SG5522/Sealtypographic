namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 會計群組變更資料
    /// </summary>
    public class AccountantGroupChangeForm
    {
        /// <summary>
        /// 會計師ID
        /// </summary>   
        /// <example>ACC001</example>
        public string Id { get; set; } = null!;

        /// <summary>
        /// 會計師群組ID
        /// 0 無群組
        /// </summary>
        /// <example>0</example>
        public string AccountantGroupId { get; set; }
    }
}

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 會計群組變更資料
    /// </summary>
    public class AccountantGroupChangeForm
    {
        /// <summary>
        /// 會計師編號
        /// </summary>   
        /// <example>ACC001</example>
        public string AccountantNumber { get; set; }

        /// <summary>
        /// 會計師群組ID
        /// NO000 無群組
        /// </summary>
        /// <example>NO000</example>
        public string AccountantGroupId { get; set; }
    }
}

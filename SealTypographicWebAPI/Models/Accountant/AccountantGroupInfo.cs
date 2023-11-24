namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師群組資料
    /// </summary>
    public class AccountantGroupInfo
    {
        /// <summary>
        /// 會計師群組名稱
        /// </summary>
        public int AccountantGroupId { get; set; }

        /// <summary>
        /// 會計師群組名稱
        /// </summary>
        /// <example>台北群組</example>
        public string AccountantGroupName { get; set; }
    }
}

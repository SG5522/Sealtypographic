namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 會計資料
    /// </summary>
    public class AccountantData
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 會計師組群ID
        /// </summary>
        public string AccountantGroupsID { get; set; } = null!;

        /// <summary>
        /// 會計師群組名稱
        /// </summary>
        public string? AccountantGroupName {get; set; }
    }
}

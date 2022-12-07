namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計資料
    /// </summary>
    public class AccountantViewModel
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
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 會計師群組名稱
        /// </summary>
        public string AccountantGroupName { get; set; }

    }
}

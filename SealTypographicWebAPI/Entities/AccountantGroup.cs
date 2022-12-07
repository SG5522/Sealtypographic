namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 會計師群組
    /// </summary>
    public class AccountantGroup : BaseNameData
    {
        /// <summary>
        /// 會計師群組編號
        /// </summary>
        public string AccountantGroupNumber { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public List<Accountant> Accountants { get; set; }
    }
}

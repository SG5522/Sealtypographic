namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 會計師印鑑簽名組歷程資料表
    /// </summary>
    public class AccountantSignJournal : SealJournal
    {      
        /// <summary>
        /// 會計師ID
        /// </summary>
        public string AccountantId { get; set; } = null!;

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public Accountant Accountant { get; set; }

    }
}

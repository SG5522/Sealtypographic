using SealTypographicWebAPI.Entities.PublicModel;

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
        public int AccountantId { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public Accountant Accountant { get; set; }

    }
}

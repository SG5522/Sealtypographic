using SealTypographicWebAPI.Entities.PublicModel;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 會計師簽名與印鑑位置
    /// </summary>
    public class AccountantSingLocation : Location
    {
        /// <summary>
        /// 會計師印鑑簽名ID
        /// </summary>
        public int AccountantSignJournalId { get; set; }

        /// <summary>
        /// 會計師印鑑簽名歷程
        /// </summary>
        public AccountantSignJournal AccountantSignJournal { get; set; }
    }
}

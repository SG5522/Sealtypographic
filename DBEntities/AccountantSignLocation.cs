using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 會計師簽名與印鑑位置
    /// </summary>
    public class AccountantSignLocation : PageLocation
    {
        /// <summary>
        /// 會計師印鑑簽名歷程
        /// </summary>
        public AccountantSignJournal AccountantSignJournal { get; set; }
    }
}

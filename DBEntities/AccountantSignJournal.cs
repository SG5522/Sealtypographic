using DBEntities.Base;
using DBEntities.Consts;

namespace DBEntities
{
    /// <summary>
    /// 會計師簽印歷程表
    /// </summary>
    public class AccountantSignJournal : BaseSealJournal
    {
        /// <summary>
        /// 會計師簽印類別
        /// </summary>
        public AccountantSignType ConfigType { get; set; }

        /// <summary>
        /// 會計師簽名與印鑑位置
        /// </summary>
        public List<AccountantSignLocation> AccountantSignLocations { get; set; }

        /// <summary>
        /// 會計師簽印建立日期歷程表
        /// </summary>
        public AccountantSignGroupJournal AccountantSignGroupJournal { get; set; }
    }
}

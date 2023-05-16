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
        /// 各印鑑簽印排版位置
        /// </summary>
        public List<TypographicSealLocation> TypographicSealLocations { get; set; }

        /// <summary>
        /// 會計師簽印建立日期歷程表
        /// </summary>
        public AccountantSignGroupJournal AccountantSignGroupJournal { get; set; }
    }
}

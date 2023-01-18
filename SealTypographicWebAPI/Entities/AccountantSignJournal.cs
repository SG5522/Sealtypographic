using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
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
        /// 會計師簽印建立日期歷程表
        /// </summary>
        public AccountantSignCreateDateJournal AccountantSignCreateDateJournal { get; set; }
    }
}

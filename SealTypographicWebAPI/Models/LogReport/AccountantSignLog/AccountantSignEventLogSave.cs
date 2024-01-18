using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.AccountantSignLog
{
    /// <summary>
    /// 印鑑異動紀錄
    /// </summary>
    public class AccountantSignEventLogSave : AccountantLog
    {
        /// <summary>
        /// 會計師編號
        /// </summary>
        public string AccountantCode { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }
    }
}

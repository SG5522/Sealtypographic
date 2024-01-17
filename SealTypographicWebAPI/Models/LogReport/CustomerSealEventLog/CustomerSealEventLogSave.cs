using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog
{
    /// <summary>
    /// 印鑑異動紀錄
    /// </summary>
    public class CustomerSealEventLogSave : CustomerLog
    {
        /// <summary>
        /// 公司編號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }
    }
}

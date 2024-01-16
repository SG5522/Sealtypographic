using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.CustomerSealGroupLog
{
    /// <summary>
    /// 印鑑異動紀錄
    /// </summary>
    public class CustomerSealGroupLogSave : CustomerLog
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

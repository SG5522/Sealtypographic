using DBEntities.Consts;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 系統設定
    /// </summary>
    public class SystemConfigOption
    {
        /// <summary>
        /// 是否啟用財報功能
        /// </summary>
        public bool FinancialReportEnabled { get; set; } 

        /// <summary>
        /// 是否啟用稅報功能
        /// </summary>
        public bool TaxReportEnabled { get; set; }

    }
}

using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class CustomerSealEventLogViewModel : LogViewModelBase
    {
        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }

        /// <summary>
        /// 公司編號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 公司姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 公曆用的季度字串
        /// </summary>
        public string DisplayQuarterYear { get; set; }
    }
}

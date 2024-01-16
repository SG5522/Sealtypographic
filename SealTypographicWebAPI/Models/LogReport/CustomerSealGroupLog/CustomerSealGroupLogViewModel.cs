using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.SealGroupLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class CustomerSealGroupLogViewModel : LogViewModelBase
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
        /// 會計師姓名
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 公曆用的季度字串
        /// </summary>
        public string DisplayQuarterYear { get; set; }
    }
}

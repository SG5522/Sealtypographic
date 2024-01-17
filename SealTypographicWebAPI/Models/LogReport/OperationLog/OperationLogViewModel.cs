using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.LogReport.OperationLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class OperationLogViewModel : LogViewModelBase
    {
        /// <summary>
        /// 動作類型
        /// </summary>
        public ActionType ActionType { get; set; }

        /// <summary>
        /// 查詢對象
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 公曆用的季度字串
        /// </summary>
        public string DisplayQuarterYear { get; set; }
    }
}

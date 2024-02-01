using SealTypographicWebAPI.Consts;
using System.ComponentModel.DataAnnotations;

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
        [Display(Order = 4)]
        public ActionType ActionType { get; set; }

        /// <summary>
        /// 查詢對象
        /// </summary>
        [Display(Order = 5)]
        public string TargetName { get; set; }

        /// <summary>
        /// 公曆用的季度字串
        /// </summary>
        [Display(Order = 6)]
        public string DisplayQuarterYear { get; set; }
    }
}

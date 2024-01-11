using CommonLib.Models;
using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class OperationLogViewModel : OperationLogSave
    {
        /// <summary>
        /// 作業時間
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}

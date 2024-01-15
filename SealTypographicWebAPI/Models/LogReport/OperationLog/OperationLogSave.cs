using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.LogReport.OperationLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class OperationLogSave : LogCustomerBase
    {
        /// <summary>
        /// 動作類型
        /// </summary>
        public ActionType ActionType { get; set; }

        /// <summary>
        /// 會計師Id
        /// </summary>
        public int AccountantId { get; set; }

        /// <summary>
        /// 會計師姓名
        /// </summary>
        public string AccountantName { get; set; }

        /// <summary>
        /// 會計師簽印群組Id
        /// </summary>
        public int AccountantSignGroupId { get; set; }

        /// <summary>
        /// 會計師簽印群組建立日期
        /// </summary>
        public DateTime AccountantSignGroupCreateDate { get; set; }
    }
}

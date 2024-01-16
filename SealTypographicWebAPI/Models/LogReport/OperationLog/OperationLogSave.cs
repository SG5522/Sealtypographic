using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.LogReport.OperationLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class OperationLogSave
    {
        /// <summary>
        /// 動作類型
        /// </summary>
        public ActionType ActionType { get; set; }

        /// <summary>
        /// 客戶Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 客戶印鑑群組Id
        /// </summary>
        public int CustomerSealGroupId { get; set; }

        /// <summary>
        /// 季度Id
        /// </summary>
        public int QuarterYearId { get; set; }

        /// <summary>
        /// 公曆年季度
        /// </summary>
        public string GregorainQuarterYear { get; set; }

        /// <summary>
        /// 年季度顯示(目前顯示民國年)
        /// </summary>
        public string DisplayQuarterYear { get; set; }

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

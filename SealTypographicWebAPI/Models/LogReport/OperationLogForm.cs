using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class OperationLogForm
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
        /// 會計師Id
        /// </summary>
        public int AccountantId { get; set; }

        /// <summary>
        /// 會計師姓名
        /// </summary>
        public string AccountantName { get; set; }

        /// <summary>
        /// 公曆用的季度字串
        /// </summary>
        public string GregorainQuarter { get; set; }

    }
}

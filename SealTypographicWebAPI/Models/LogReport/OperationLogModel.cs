namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class OperationLogModel
    {
        /// <summary>
        /// 客戶Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 公曆用的季度Id
        /// </summary>
        public int QuarterYearId { get; set; }

        /// <summary>
        /// 公曆用的季度字串
        /// </summary>
        public string GregorainQuarter { get; set; }
        
    }
}

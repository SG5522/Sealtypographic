namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 紀錄使用TypographicPDF Service時所讀取的資料
    /// </summary>
    public class LogCustomerBase
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
        /// 客戶印鑑群組Id
        /// </summary>
        public int CustomerSealGroupId { get; set; }

        /// <summary>
        /// 季度Id
        /// </summary>
        public int QuarterYearId { get; set; }

        /// <summary>
        /// 公曆用的季度字串
        /// </summary>
        public string GregorainQuarter { get; set; }
    }    
}

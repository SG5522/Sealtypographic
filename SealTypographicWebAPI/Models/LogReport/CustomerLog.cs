namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 紀錄使用TypographicPDF Service時所讀取的資料
    /// </summary>
    public class CustomerLog
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
        /// 公曆年季度
        /// </summary>
        public string GregorainQuarterYear { get; set; }

        /// <summary>
        /// 年季度顯示(目前顯示民國年)
        /// </summary>
        public string DisplayQuarterYear { get; set; }
    }    
}

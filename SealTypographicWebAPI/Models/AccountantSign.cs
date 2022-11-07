namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 會計師印鑑簽名組
    /// </summary>
    public class AccountantSign
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public string AccountantID { get; set; } = null!;

        /// <summary>
        /// 會計師簽名群組
        /// 1.印鑑
        /// 2.中文簽名
        /// 3.英文簽名
        /// 4.舊式簽名(英文)        
        /// </summary>
        public int AccountantSignGroupID { get; set; }

        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// 啟用日(審查通過才有)
        /// </summary>
        public DateOnly AvailableDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateOnly CreatedDate { get; set; }
    }
}

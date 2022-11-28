namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師印鑑簽名
    /// </summary>
    public class AccountantSignPost
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public string AccountantID { get; set; }

        /// <summary>
        /// 會計師簽名群組
        /// 1.印鑑
        /// 2.中文簽名
        /// 3.英文簽名
        /// 4.舊式簽名(英文)        
        /// </summary>
        public int SealMappingConfigId { get; set; }

        /// <summary>
        /// 圖檔
        /// </summary>
        public string ImageBase64 { get; set; }

        /// <summary>
        /// 啟用日(審查通過才有)
        /// </summary>
        public DateTime AvailableDate { get; set; }

        /// <summary>
        /// 啟用結束日期
        /// </summary>
        public DateTime DeadlineDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 狀態
        /// 0.待審查
        /// 1.通過(審核完成)
        /// 2.退件
        /// 3.刪除(系統管理員可以看到資料)
        /// </summary>
        public int Status { get; set; }

    }
}

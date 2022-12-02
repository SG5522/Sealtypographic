namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師印鑑簽名
    /// </summary>
    public class AccountantSignForm : PostCreateData
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
        /// 啟用結束日期
        /// </summary>
        public DateTime DeadlineDate { get; set; }

    }
}

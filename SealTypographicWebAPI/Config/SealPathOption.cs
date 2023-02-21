namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 匯入AppConfig資料
    /// </summary>
    public class SealPathOption
    {
        /// <summary>
        /// 印鑑路徑
        /// </summary>
        public string SealRootPath { get; set; } = string.Empty;

        /// <summary>
        /// 客戶印鑑放置路徑
        /// </summary>
        public string Customer
        {
            get
            {
                return Path.Combine(SealRootPath, "Customer");
            }
        }
            

        /// <summary>
        /// 會計師簽印放置路徑
        /// </summary>
        public string Accountant
        {
            get
            {
                return Path.Combine(SealRootPath, "Accountant");
            }
        }

        /// <summary>
        /// 信頭放置路徑
        /// </summary>
        public string Letterhead {
            get
            {
                return Path.Combine(SealRootPath, "Letterhead");
            }
        }

        /// <summary>
        /// 縮圖比例
        /// </summary>
        public float ResizeScale { get; set; }

    }
}

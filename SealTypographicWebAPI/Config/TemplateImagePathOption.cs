namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 匯入AppConfig資料
    /// </summary>
    public class TemplateImagePathOption
    {
        /// <summary>
        /// 印鑑路徑
        /// </summary>
        public string RootPath { get; set; } = string.Empty;

        /// <summary>
        /// 客戶印鑑放置路徑
        /// </summary>
        public string Customer
        {
            get
            {
                return Path.Combine(RootPath, "Customer");
            }
        }
            

        /// <summary>
        /// 會計師簽印放置路徑
        /// </summary>
        public string Accountant
        {
            get
            {
                return Path.Combine(RootPath, "Accountant");
            }
        }

        /// <summary>
        /// 信頭放置路徑
        /// </summary>
        public string Letterhead {
            get
            {
                return Path.Combine(RootPath, "Letterhead");
            }
        }

        /// <summary>
        /// 縮圖比例
        /// </summary>
        public float ResizeScale { get; set; }

    }
}

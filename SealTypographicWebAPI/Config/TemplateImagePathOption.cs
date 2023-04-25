using DBEntities.Consts;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 匯入AppSetting資料
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
                return Path.Combine(RootPath, SealType.Customer.ToString());
            }
        }
            

        /// <summary>
        /// 會計師簽印放置路徑
        /// </summary>
        public string Accountant
        {
            get
            {
                return Path.Combine(RootPath, SealType.Accountant.ToString());
            }
        }

        /// <summary>
        /// 信頭放置路徑
        /// </summary>
        public string Letterhead {
            get
            {
                return Path.Combine(RootPath, SealType.Letterhead.ToString());
            }
        }

        /// <summary>
        /// 縮圖比例
        /// </summary>
        public float ResizeScale { get; set; }

    }
}

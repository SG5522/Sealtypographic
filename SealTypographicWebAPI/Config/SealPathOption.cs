using DBEntities.Consts;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 匯入AppSetting資料
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
                return Path.Combine(SealRootPath, SealType.Customer.ToString());
            }
        }
            

        /// <summary>
        /// 會計師簽印放置路徑
        /// </summary>
        public string Accountant
        {
            get
            {
                return Path.Combine(SealRootPath, SealType.Accountant.ToString());
            }
        }

        /// <summary>
        /// 信頭放置路徑
        /// </summary>
        public string Letterhead {
            get
            {
                return Path.Combine(SealRootPath, SealType.Letterhead.ToString());
            }
        }

        /// <summary>
        /// 臨時章放置路徑
        /// </summary>
        public string TemporarySeal
        {
            get
            {
                return Path.Combine(SealRootPath, SealType.TemporarySeal.ToString());
            }            
        }

        /// <summary>
        /// 縮圖比例
        /// </summary>
        public float ResizeScale { get; set; }

    }
}

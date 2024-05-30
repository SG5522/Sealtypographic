using DBEntities.Consts;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 匯入AppConfig資料
    /// </summary>
    public class UploadPathOption
    {
        /// <summary>
        /// 上傳資料根目錄放置路徑
        /// </summary>
        public string UploadRootPath { get; set; } = string.Empty;

        /// <summary>
        /// 客戶印鑑授權書放置路徑
        /// </summary>
        public string CustomerSealAuthorization 
        {
            get
            {
                return Path.Combine(UploadRootPath, UploadType.CustomerSealAuthorization.ToString());
            }
        }

        /// <summary>
        /// 會計師授權書放置路徑
        /// </summary>
        public string AccountantSignAuthorization
        {
            get
            {
                return Path.Combine(UploadRootPath, UploadType.AccountantSignAuthorization.ToString());
            }
        }

        /// <summary>
        /// 信頭放置路徑
        /// </summary>
        public string LetterheadImage
        {
            get
            {
                return Path.Combine(UploadRootPath, UploadType.LetterheadImage.ToString());
            }
        }

        /// <summary>
        /// 財務報表的PDF倒放置路徑
        /// </summary>
        public string FinancialReport
        {
            get
            {
                return Path.Combine(UploadRootPath, UploadType.FinancialReport.ToString());
            }
        }
        
        /// <summary>
        /// 稅務報表的PDF倒放置路徑
        /// </summary>
        public string TaxReport
        {
            get
            {
                return Path.Combine(UploadRootPath, UploadType.TaxReport.ToString());
            }
        }

        /// <summary>
        /// 會計師證明書放置路徑
        /// </summary>
        public string AccountantSignCertificate
        {
            get
            {
                return Path.Combine(UploadRootPath, UploadType.AccountantSignCertificate.ToString());
            }
        }

        /// <summary>
        /// 臨時檔放置路徑
        /// </summary>
        public string Temporary
        {
            get
            {
                return Path.Combine(UploadRootPath, UploadType.Temporary.ToString());
            }
        }

        /// <summary>
        /// 檔案上傳大小限制
        /// </summary>
        public int MaxUploadSize { get; set; }

    }
}

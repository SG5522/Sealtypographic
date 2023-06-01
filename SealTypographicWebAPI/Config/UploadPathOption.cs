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
        /// PDF放置路徑
        /// </summary>
        public string PDF
        {
            get
            {
                return Path.Combine(UploadRootPath, UploadType.PDF.ToString());
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

    }
}

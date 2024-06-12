using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 上傳後加密處理的檔案及加密key
    /// </summary>
    public class UploadEncryptFile
    {
        /// <summary>
        /// 檔案放置位置
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 加密key
        /// </summary>
        public string EncryptKey { get; set; }

        /// <summary>
        /// 上傳檔案類別
        /// </summary>
        public UploadType UploadType { get; set; }

        /// <summary>
        /// RSAKey
        /// </summary>
        public RSAKey RSAKey { get; set; }
    }
}

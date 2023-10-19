using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 上傳掃描內容
    /// </summary>
    public class UploadScanData
    {
        /// <summary>
        /// 上傳類別 
        /// </summary>
        public UploadType UploadType { get; set; }

        /// <summary>
        /// 圖檔字串
        /// </summary>
        public List<string> ImageBase64Strings { get; set; }
    }
    
}

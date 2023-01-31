using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 上傳檔案內容
    /// </summary>
    public class UploadData
    {
        /// <summary>
        /// 上傳類別 
        /// </summary>
        public UploadType UploadType { get; set; }

        /// <summary>
        /// 上傳重複檔名處理模式
        /// 0.無重複
        /// 1.保留
        /// 2.覆蓋
        /// </summary>
        public DuplicateFileProcessMode DuplicateFileProcessMode { get; set; }

        /// <summary>
        /// 重複檔案Id
        /// </summary>
        public List<int>? DuplicateFileIds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<IFormFile> FormFiles { get; set; }
    }
    
}

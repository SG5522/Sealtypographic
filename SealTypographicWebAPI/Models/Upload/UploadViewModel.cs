using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 上傳檔案
    /// </summary>
    public class UploadViewModel : UploadFileViewModel
    {        
        /// <summary>
        /// 上傳檔案類別
        /// </summary>
        public UploadType UploadType { get; set; }

        /// <summary>
        /// 檔案工作狀態
        /// </summary>
        public FileWorkStatus FileWorkStatus { get; set; }
    }    
}

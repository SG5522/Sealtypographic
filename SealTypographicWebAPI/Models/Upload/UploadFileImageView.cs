using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 上傳檔案
    /// </summary>
    public class UploadFileImageView : ResponseViewModel
    {        
        /// <summary>
        /// 檔名
        /// </summary>
        public string ImageBase64 { get; set; }
    }
}

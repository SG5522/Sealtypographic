using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 上傳檔案
    /// </summary>
    public class UploadFileViewModel : BaseData
    {        
        /// <summary>
        /// 上傳時間
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// 檔名
        /// </summary>
        public string FileName { get; set; }
    }

    /// <summary>
    /// 上傳 (IFromFile)
    /// </summary>
    public class UploadFileResponse : ResponseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public UploadFileResponse()
        {
            ViewModel = new();
        }

        /// <summary>
        /// 已上傳的檔案列表
        /// </summary>
        public List<UploadFileViewModel> ViewModel { get; set; }
    }
}

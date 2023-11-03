using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Upload
{
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

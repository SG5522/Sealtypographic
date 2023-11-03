

using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 取得上傳類別並回應訊息
    /// </summary>
    public class UploadPaginateViewModel : PaginateViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public UploadPaginateViewModel()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 上傳類別
        /// </summary>
        public List<UploadViewModel> ViewModels { get; set; }
    }
}

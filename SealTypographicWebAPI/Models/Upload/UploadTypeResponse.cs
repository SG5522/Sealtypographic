using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 取得上傳類別並回應訊息
    /// </summary>
    public class UploadTypeResponse : ResponseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public UploadTypeResponse()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 上傳類別
        /// </summary>
        public List<UploadTypeViewModel> ViewModels { get; set; }
    }
}

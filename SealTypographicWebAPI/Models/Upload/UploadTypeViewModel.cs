using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 上傳類別
    /// </summary>
    public class UploadTypeViewModel
    {
        /// <summary>
        /// 上傳類型
        /// </summary>
        public UploadType UploadType { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string? Name { get; set; }
    }

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

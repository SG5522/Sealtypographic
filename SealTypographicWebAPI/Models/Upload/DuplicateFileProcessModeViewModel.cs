using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 上傳重複檔名處理模式
    /// </summary>
    public class DuplicateFileProcessModeViewModel
    {
        /// <summary>
        /// 上傳重複檔名處理模式
        /// </summary>
        public DuplicateFileProcessMode DuplicateFileProcessMode { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// 取得上傳類別並回應訊息
    /// </summary>
    public class DuplicateFileProcessModeResponse : ResponseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public DuplicateFileProcessModeResponse()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 上傳類別
        /// </summary>
        public List<DuplicateFileProcessModeViewModel> ViewModels { get; set; }
    }
}

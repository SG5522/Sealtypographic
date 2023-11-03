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
}

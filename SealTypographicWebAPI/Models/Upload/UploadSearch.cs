using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 上傳檔案搜尋
    /// </summary>
    public class UploadSearch : PaginateSearch
    {
        /// <summary>
        /// 關鍵字
        /// </summary>        
        /// <example>CUS001 or A公司</example>
        public string? KeyWord { get; set; }

        /// <summary>
        /// 上傳檔案類別
        /// </summary>
        public UploadType? UploadType { get; set; }

        /// <summary>
        /// 檔案工作狀態
        /// </summary>
        public FileWorkStatus? FileWorkStatus { get; set; }
    }
}

using EFCore.BulkExtensions;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 上傳檔案資料表
    /// </summary>
    public class UploadFile : BaseData
    {
        /// <summary>
        /// 上傳檔案類別
        /// </summary>
        public UploadType UploadType { get; set; }

        /// <summary>
        /// 檔名
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 檔案工作狀態
        /// 在使用客戶印鑑、會計師簽印、信頭、PDF檔案
        /// 為了不在重複使用同一份檔案所做的狀態區分
        /// </summary>
        public FileWorkStatus FileWorkStatus { get; set; }
    }
}

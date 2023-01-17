using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 事務所信頭圖片歷程資料表
    /// </summary>
    public class LetterheadImageJournal : BaseData
    {        
        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string ImageFullPath { get; set; }

        /// <summary>
        /// 信頭圖片啟用狀態
        ///  0.啟用
        /// 10.停用
        /// </summary>
        public LetterheadImageStatus Status { get; set; }

        /// <summary>
        /// 信頭圖片建立日期歷程
        /// </summary>
        public Letterhead Letterhead { get; set; }
    }
}

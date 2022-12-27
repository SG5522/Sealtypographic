using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 事務所信頭圖片歷程資料表
    /// </summary>
    public class LetterheadImageJournal : SealJournal
    {
        /// <summary>
        ///  信頭圖片編號(排序) 1為起始
        /// </summary>
        public int Sequence { get; set; }
        
        /// <summary>
        /// 信頭ID
        /// </summary>
        public int LetterheadId { get; set; }

        /// <summary>
        /// 信頭圖片群組創建日期
        /// </summary>
        public DateTime GroupCreateDate { get; set; }

        /// <summary>
        /// 事務所信頭資料表
        /// </summary>
        public Letterhead Letterhead { get; set; }

    }
}

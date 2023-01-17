using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 事務所信頭圖片建立日期歷程資料表
    /// </summary>
    public class LetterheadImageCreateDateJournal : BaseReviewData
    {        
        /// <summary>
        /// 信頭基本資料表
        /// </summary>
        public Letterhead Letterhead { get; set; }

        /// <summary>
        /// 印鑑組歷程資料表
        /// </summary>
        public List<LetterheadImageJournal> LetterheadImageJournals { get; set; }
    }
}

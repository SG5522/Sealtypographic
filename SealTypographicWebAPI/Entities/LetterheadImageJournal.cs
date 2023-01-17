using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 事務所信頭圖片歷程資料表
    /// </summary>
    public class LetterheadImageJournal : BaseSealJournal
    {        
        /// <summary>
        /// 信頭圖片建立日期歷程
        /// </summary>
        public LetterheadImageCreateDateJournal LetterheadImageCreateJournal { get; set; }
    }
}

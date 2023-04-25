using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 事務所信頭資料表
    /// </summary>
    public class Letterhead : BaseNameData
    {
        /// <summary>
        /// 會計師事務所
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// 信頭圖片歷程
        /// </summary>
        public List<LetterheadImageJournal> LetterheadImageJournals { get; set; }
    }
}

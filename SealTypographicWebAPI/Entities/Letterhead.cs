using SealTypographicWebAPI.Entities.PublicModel;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 事務所信頭資料表
    /// </summary>
    public class Letterhead : BaseNameDeleteStatusData
    {
        /// <summary>
        /// 信頭圖片資料(歷程)
        /// </summary>
        public List<LetterheadImageJournal> LetterheadImageJournals { get; set; }
    }
}

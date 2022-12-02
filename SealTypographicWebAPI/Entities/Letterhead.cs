namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 事務所信頭資料表
    /// </summary>
    public class Letterhead : BaseData
    {
        /// <summary>
        /// 信頭圖片資料(歷程)
        /// </summary>
        public List<LetterheadImageJournal> LetterheadImageJournals { get; set; }
    }
}

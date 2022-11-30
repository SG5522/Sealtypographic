namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 信頭圖片排版位置
    /// </summary>
    public class LetterheadImageLocaltion : Localtion
    {
        /// <summary>
        /// 信頭圖片ID
        /// </summary>
        public int LetterheadImageJournalId { get; set; }

        /// <summary>
        /// 信頭圖片歷程
        /// </summary>
        public LetterheadImageJournal LetterheadImageJournal { get; set; }
    }
}

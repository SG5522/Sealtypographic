namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 信頭圖片排版位置
    /// </summary>
    public class LetterheadImageLocationForm : SealLocationForm
    {
        /// <summary>
        /// 信頭圖片ID
        /// </summary>
        public int LetterheadImageJournalId { get; set; }

    }
}

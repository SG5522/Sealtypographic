namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 建完信頭後回傳ID
    /// </summary>
    public class LetterheadCreateResronse : ResponseViewModel
    {
        /// <summary>
        /// 信頭Id
        /// </summary>        
        public int LetterheadId { get; set; }
    }
}

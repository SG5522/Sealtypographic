namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// /// 信頭資料列表
    /// </summary>
    public class LetterheadResponse : ResponseViewModel
    {
        /// <summary>
        /// 信頭資料
        /// </summary>
        public LetterheadViewModel ViewModel { get; set; }
    }
}

using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 信頭圖片排版位置
    /// </summary>
    public class LetterheadImageLocation : PageLocation
    {        
        /// <summary>
        /// 信頭圖片歷程
        /// </summary>
        public List<LetterheadImageJournal> LetterheadImageJournal { get; set; }
    }
}

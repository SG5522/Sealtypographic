using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 信頭圖片排版位置
    /// </summary>
    public class LetterheadImageLocation : BasePageLocation
    {        
        /// <summary>
        /// 信頭圖片歷程
        /// </summary>
        public LetterheadImageJournal LetterheadImageJournal { get; set; }
    }
}

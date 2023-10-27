using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 信頭圖片排版位置
    /// </summary>
    public class LetterheadImageLocationViewModel : BaseSealLocation
    {
        /// <summary>
        /// 信頭名稱
        /// </summary>
        public string LetterheadName { get; set; }
    }
}

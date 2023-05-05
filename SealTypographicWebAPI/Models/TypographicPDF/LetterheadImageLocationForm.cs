using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 信頭圖片排版位置
    /// </summary>
    public class LetterheadImageLocationForm : BaseLocationModel
    {
        /// <summary>
        /// 信頭圖片ID
        /// </summary>
        public int LetterheadImageId { get; set; }

    }
}

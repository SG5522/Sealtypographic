using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭資料
    /// </summary>
    public class LetterheadViewModel : BaseName
    {
        /// <summary>
        /// 信頭圖片Id
        /// </summary>
        public int LetterheadImageId { get; set; }
    }
}

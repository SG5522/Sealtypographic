using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭資料
    /// </summary>
    public class LetterheadViewModel : BaseData
    {
        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>信頭A</example>
        public string Name { get; set; }

        /// <summary>
        /// 信頭圖片Id
        /// </summary>
        public int LetterheadImageId { get; set; }
    }
}

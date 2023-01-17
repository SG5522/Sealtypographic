using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖片搜尋(信頭圖片ID)
    /// </summary>
    public class LetterheadImageSearch
    {
        /// <summary>
        /// 信頭建立日期Id
        /// </summary>
        /// <example>1</example>
        [Required]
        public int LetterheadImageId { get; set; }

    }
}

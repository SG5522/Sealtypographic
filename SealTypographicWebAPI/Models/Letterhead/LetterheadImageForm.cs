using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖片
    /// </summary>
    public class LetterheadImageForm
    {
        /// <summary>
        /// 信頭名稱
        /// </summary>
        /// <example>1</example>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        [Required]
        public string ImageBase64 { get; set; }

    }
}

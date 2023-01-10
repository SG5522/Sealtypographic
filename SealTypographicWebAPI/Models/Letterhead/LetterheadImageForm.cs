using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖片
    /// </summary>
    public class LetterheadImageForm : BaseCreateSeal
    {
        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>
        [Required]
        [Range(1, 99)]
        public int Sequence { get; set; }
    }
    /// <summary>
    /// 信頭圖片組
    /// </summary>
    public class LetterheadImageForms
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        /// <example>1</example>
        [Required]
        public int LetterheadId { get; set; }

        /// <summary>
        /// 信頭圖片組
        /// </summary>
        public List<LetterheadImageForm> ImageForms { get; set; }
    }
}

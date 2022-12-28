using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖片資料(含ID)
    /// </summary>
    public class LetterheadImageFormUpdate : BaseUpdateSeal
    {
        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>        
        [Required]
        [Range(1, 99)]
        public int Sequence { get; set; }
    }
}

using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖片組
    /// </summary>
    public class LetterheadImageViewModel: ResponseViewModel
    {       
        /// <summary>
        /// 信頭圖片ID
        /// </summary>
        /// <example>0</example>        
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }
    }
}

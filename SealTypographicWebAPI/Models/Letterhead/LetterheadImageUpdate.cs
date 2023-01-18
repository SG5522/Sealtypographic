using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.Customer;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 異動信頭圖片資料
    /// </summary>
    public class LetterheadImageUpdate : BaseUpdateSeal
    {
        /// <summary>
        /// 信頭名稱
        /// </summary>        
        [Required]
        public string LetterheadName { get; set; }
    }
}

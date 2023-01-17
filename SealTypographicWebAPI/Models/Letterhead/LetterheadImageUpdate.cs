using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.Customer;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 更新信頭圖片組資料
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

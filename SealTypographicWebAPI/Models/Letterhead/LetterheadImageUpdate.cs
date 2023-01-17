using SealTypographicWebAPI.Models.Customer;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 更新信頭圖片組資料
    /// </summary>
    public class LetterheadImageUpdate
    {
        /// <summary>
        /// 信頭Id
        /// </summary>        
        [Required]
        public int LetterheadId { get; set; }

        /// <summary>
        /// 信頭圖片建立日期Id
        /// </summary>        
        [Required]
        public int LetterheadImageCreateDateId { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime GroupCreateDate { get; set; }

        /// <summary>
        /// 更新客戶印鑑列表
        /// </summary>
        public List<LetterheadImageFormUpdate> UpdateLetterheadImages { get; set; }

        /// <summary>
        /// 新增客戶印鑑列表
        /// </summary>
        public List<LetterheadImageForm> CreateLetterheadImages { get; set; }
    }
}

using SealTypographicWebAPI.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖片群組創建日期
    /// </summary>
    public class LetterheadImageCreateDateView
    {
        /// <summary>
        /// 信頭圖片建立日期Id
        /// </summary>
        /// <example>1</example>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 群組創建日期
        /// </summary>
        /// <example>0001/01/01 00:00:00</example>
        [Required]
        public DateTime GroupCreateDate { get; set; }

        /// <summary>
        /// 審查狀態
        /// </summary>
        [Required]
        public LetterheadImageStatus Status { get; set; }
    }

    /// <summary>
    /// 信頭名稱與圖片群組創建日期列表
    /// </summary>
    public class LetterheadImageCreateDateViews : ResponseViewModel
    {
        /// <summary>
        /// 信頭名稱
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 信頭圖片創建日期搜尋表
        /// </summary>
        public List<LetterheadImageCreateDateView> CreateDateViews { get; set; }
    }
}

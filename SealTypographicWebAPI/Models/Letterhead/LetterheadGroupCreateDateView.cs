using SealTypographicWebAPI.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖片群組創建日期
    /// </summary>
    public class LetterheadGroupCreateDateView
    {
        /// <summary>
        /// 會計師Id
        /// </summary>
        /// <example>1</example>
        [Required]
        public int LetterheadId { get; set; }

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
        public ReviewStatus ReviewStatus { get; set; }
    }

    /// <summary>
    /// 信頭圖片群組創建日期列表
    /// </summary>
    public class LetterheadGroupCreateDateViews : ResponseViewModel
    {
        /// <summary>
        /// 信頭圖片創建日期搜尋表
        /// </summary>
        public List<LetterheadGroupCreateDateView> LetterheadGroupCreateDateView { get; set; }
    }
}

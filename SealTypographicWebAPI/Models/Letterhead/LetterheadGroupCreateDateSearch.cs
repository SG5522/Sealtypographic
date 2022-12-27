using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖片搜尋(依信頭ID與群組創建日期)
    /// </summary>
    public class LetterheadGroupCreateDateSearch
    {
        /// <summary>
        /// 信頭Id
        /// </summary>
        /// <example>1</example>
        [Required]
        public int LetterheadId { get; set; }

        /// <summary>
        /// 啟用時間
        /// </summary>
        /// <example>0001-01-01T00:00:00.000000</example>
        [Required]
        public DateTime GroupCreateDate { get; set; }
    }
}

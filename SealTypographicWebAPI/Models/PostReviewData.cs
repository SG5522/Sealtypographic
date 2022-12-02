using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 各項目會使用到的基本資料
    /// </summary>
    public class PostReviewBaseData
    {
        /// <summary>
        /// 啟用日期
        /// </summary>
        /// <example>0000-01-01T00:00:00.000Z</example>
        public DateTime AvailableDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章資料
    /// </summary>
    public abstract class TemporarySealBaseForm
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        /// <example>1</example>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// 臨時章名稱
        /// </summary>
        /// <example>臨時章01</example>
        [Required]
        public string Quarter { get; set; }
    }
}

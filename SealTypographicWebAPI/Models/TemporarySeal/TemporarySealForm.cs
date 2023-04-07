using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Temporary
{
    /// <summary>
    /// 臨時章
    /// </summary>
    public class TemporarySeal : BaseCreateSeal
    {
        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>
        [Required]
        [Range(1, 99)]
        public int Sequence { get; set; }
    }

    /// <summary>
    /// 臨時章資料
    /// </summary>
    public class TemporarySealForm
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
        public string Name { get; set; }

        /// <summary>
        /// 臨時印鑑組
        /// </summary>
        [Required]
        public List<TemporarySeal> Seals { get; set; }
    }
}

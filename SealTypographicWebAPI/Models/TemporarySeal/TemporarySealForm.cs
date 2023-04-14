using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章(新增使用)
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
    public class TemporarySealForm : TemporarySealBaseForm
    {
        /// <summary>
        /// 臨時印鑑組
        /// </summary>
        [Required]
        public List<TemporarySeal> Seals { get; set; }
    }
}

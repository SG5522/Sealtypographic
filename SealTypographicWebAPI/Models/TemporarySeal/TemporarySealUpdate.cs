using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 更新臨時章
    /// </summary>
    public class TemporarySealUpdate : BaseUpdateSeal
    {
        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>
        [Required]
        [Range(1, 99)]
        public int Sequence { get; set; }
    }
}

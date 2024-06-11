using SealTypographicWebAPI.Models.BaseModels;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章
    /// </summary>
    public class TemporarySealViewModel : BaseSeal
    {
        /// <summary>
        /// 印鑑編號(排序)
        /// </summary>
        public int Sequence { get; set; }
    }
}

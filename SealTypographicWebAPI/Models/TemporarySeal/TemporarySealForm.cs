using System.ComponentModel.DataAnnotations;
using DBEntities.Consts;
using System.Text.Json.Serialization;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章(新增使用)
    /// </summary>
    public class TemporarySeal : BaseCreateSeal
    {
        private int sequence;

        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>
        [Required]
        [Range(1, 99)]
        public int Sequence 
        { 
            get => sequence;
            set { 
                sequence = value;
                CommonSequence = sequence;
            }
        }

        /// <summary>
        /// 印鑑、簽印類型 
        /// </summary>
        [JsonIgnore]
        public override SealType SealType => SealType.TemporarySeal;

    }

    /// <summary>
    /// 臨時章資料
    /// </summary>
    public class TemporarySealForm : TemporarySealBaseForm
    {
        /// <summary>
        /// 臨時章印鑑組
        /// </summary>
        [Required]
        public List<TemporarySeal> Seals { get; set; }
    }
}

using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 臨時章歷程
    /// </summary>
    public class TemporarySealJournal : BaseSealJournal
    {
        /// <summary>
        /// 臨時章印鑑編號(排序)
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 臨時章群組
        /// </summary>
        public TemporarySealGroup TemporarySealGroup { get; set; }
    }
}

using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 臨時章歷程
    /// </summary>
    public class TemporarySealJournal : BaseSealJournal
    {
        /// <summary>
        /// 臨時章群組
        /// </summary>
        public Temporary TemporarySealGroup { get; set; }
    }
}

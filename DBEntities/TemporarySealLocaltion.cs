using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 臨時章排版位置
    /// </summary>
    public class TemporarySealLocation : BasePageLocation
    {        
        /// <summary>
        /// 臨時章歷程
        /// </summary>
        public TemporarySealJournal TemporarySealJournal { get; set; }
    }
}

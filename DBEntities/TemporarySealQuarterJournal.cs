using DBEntities.Base;
namespace DBEntities
{
    /// <summary>
    /// 臨時章群組
    /// </summary>
    public class TemporarySealQuarterJournal : BaseData
    {
        /// <summary>
        /// 臨時章季度
        /// </summary>
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// 臨時章歷程
        /// </summary>
        public List<TemporarySealJournal> TemporarySealJournals { get; set; }
    }
}

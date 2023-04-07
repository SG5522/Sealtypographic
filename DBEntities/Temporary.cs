using DBEntities.Base;
namespace DBEntities
{
    /// <summary>
    /// 臨時章群組
    /// </summary>
    public class Temporary : BaseNameData
    {
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

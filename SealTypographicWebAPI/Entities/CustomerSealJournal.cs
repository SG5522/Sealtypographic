namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 客戶印鑑組歷程資料表
    /// </summary>
    public class CustomerSealJournal : SealJournal
    {
        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        public string Quarter { get; set; } = null!;

        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; } = null!;

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public Customer Customer { get; set; }

    }
}

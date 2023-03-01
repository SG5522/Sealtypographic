using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 印鑑序號是否重複確認用
    /// </summary>
    public class CustomerSealSequenceCheck
    {
        /// <summary>
        /// 客戶印鑑群組ID 
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管    
        /// 5.其他(客戶)
        /// </summary>
        public CustomerSealType SealMappingConfigId { get; set; }

        /// <summary>
        /// 印鑑序號
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 客戶印鑑季度Id
        /// </summary>
        /// <example>1</example>
        public int CustomerSealQuarterJournalId { get; set; }
    }
}

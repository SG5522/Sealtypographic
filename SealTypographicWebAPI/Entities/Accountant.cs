using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 會計師資料表
    /// </summary>
    public class Accountant : BaseNameData
    {
        /// <summary>
        /// 會計編號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 會計師群組ID
        /// </summary>                
        public int AccountantGroupId { get; set; }

        /// <summary>
        /// 會計師群組
        /// </summary>
        public AccountantGroup AccountantGroup { get; set; }

        /// <summary>
        /// 會計師印鑑資料(歷程)
        /// </summary>
        public List<AccountantSignGroupJournal> AccountantSignCreateDateJournals { get; set; }
    }
}

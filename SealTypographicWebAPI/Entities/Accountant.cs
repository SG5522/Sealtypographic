using SealTypographicWebAPI.Entities.PublicModel;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 會計師資料表
    /// </summary>
    public class Accountant : BaseNameDeleteStatusData
    {
        /// <summary>
        /// 會計編號
        /// </summary>
        public string AccountantNumber { get; set; }

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
        public List<AccountantSignJournal> AccountantSignJournals { get; set; }
    }
}

using DBEntities.Entities.Base;

namespace DBEntities.Entities.AccountantModels
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
        /// 會計師事務所
        /// </summary>
        public Company Company { get; set; }


        /// <summary>
        /// 會計師群組資料表
        /// </summary>
        public IList<AccountantGroup> AccountantGroups { get; set; }

        /// <summary>
        /// 會計師與會計師群組多對多資料表
        /// </summary>
        public IList<GroupAccountant> GroupAccountants { get; set; }


        /// <summary>
        /// 會計師印鑑資料(歷程)
        /// </summary>
        public IList<AccountantSignGroup> AccountantSignGroups { get; set; }
    }
}

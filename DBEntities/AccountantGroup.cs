using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 會計師群組
    /// </summary>
    public class AccountantGroup : BaseNameData
    {
        /// <summary>
        /// 會計師群組編號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 會計師事務所(公司)資料表
        /// </summary>
        public Company Company { get; set; }


        /// <summary>
        /// 會計師資料表
        /// </summary>
        public IList<Accountant> Accountants { get; set; }

        /// <summary>
        /// 會計師與會計師群組多對多資料表
        /// </summary>
        public IList<GroupAccountant> GroupAccountants { get; set; }
    }
}

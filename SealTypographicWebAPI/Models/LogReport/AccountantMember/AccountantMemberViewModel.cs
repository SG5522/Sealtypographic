using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport.AccountantList
{
    /// <summary>
    /// 會計師成員
    /// </summary>
    public class AccountantMemberViewModel : BaseData
    {
        /// <summary>
        /// 編號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        public string GroupName { get; set; }
    }
}

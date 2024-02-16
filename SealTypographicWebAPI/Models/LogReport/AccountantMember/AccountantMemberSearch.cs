using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport.AccountantMember
{
    /// <summary>
    /// 會計師成員查詢
    /// </summary>
    public class AccountantMemberSearch : PaginateSearch
    {
        /// <summary>
        /// 會計師編號/姓名
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 會計師群組Id
        /// </summary>
        public int AccountantGroupId { get; set; }
    }
}

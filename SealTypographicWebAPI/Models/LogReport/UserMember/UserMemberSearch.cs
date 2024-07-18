using DJKeycloakAPI.Models;

namespace SealTypographicWebAPI.Models.LogReport.UserMember
{
    /// <summary>
    /// 使用者成員搜尋
    /// </summary>
    public class UserMemberSearch : PaginateQuery
    {
        /// <summary>
        /// 使用者ID(帳號)
        /// 使用者名稱
        /// 使用者姓氏
        /// </summary>
        public string? UserQuery { get; set; }

        /// <summary>
        /// 使用者群組Id
        /// </summary>
        public string? UserGroupName { get; set; }
    }
}

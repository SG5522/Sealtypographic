using DJKeycloakAPI.Models;
using System.ComponentModel.DataAnnotations;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
        [RegularExpression(@"^[a-zA-Z0-9\s\u4e00-\u9fa5 .,_'\-]*$")]
        public string? UserQuery { get; set; }

        /// <summary>
        /// 使用者群組Id
        /// </summary>
        public string? UserGroupName { get; set; }


        /// <summary>
        /// 取得增加中墜文字的keyword
        /// </summary>
        /// <returns></returns>
        public string GetSanitizedQuery() => $"*{UserQuery}*";
    }
}

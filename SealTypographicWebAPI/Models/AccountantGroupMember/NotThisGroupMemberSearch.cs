using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.PublicModel;

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 群組搜尋條件
    /// </summary>
    public class NotThisGroupMemberSearch : PaginateSearch
    {
        /// <summary>
        /// 會計師編號或名稱
        /// </summary>
        /// <example>ACC000 or 會計師名字</example>
        public string? AccountantNumberOrName { get; set; }

        /// <summary>
        /// 群組Id
        /// </summary>        
        [Required]        
        public int AccountantGroupId { get; set; }
    }
}

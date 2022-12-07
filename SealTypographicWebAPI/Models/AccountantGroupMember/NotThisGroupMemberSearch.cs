using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 群組搜尋條件
    /// </summary>
    public class NotThisGroupMemberSearch : PaginateSearch
    {
        /// <summary>
        /// ID或名稱
        /// </summary>
        public string? AccountantIdOrName { get; set; }

        /// <summary>
        /// 群組Id
        /// </summary>        
        [Required]
        [MaxLength(6)]
        public string AccountantGroupId { get; set; }
    }
}

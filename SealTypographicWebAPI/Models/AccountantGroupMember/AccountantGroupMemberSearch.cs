using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.PublicModel;

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 群組搜尋條件
    /// </summary>
    public class AccountantGroupMemberSearch : PaginateSearch
    {

        /// <summary>
        /// 群組Id
        /// </summary>        
        [Required]
        [MaxLength(6)]
        public string AccountantGroupId { get; set; }
    }
}

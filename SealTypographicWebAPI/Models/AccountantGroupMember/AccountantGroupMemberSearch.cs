using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 群組成員搜尋條件
    /// </summary>
    public class AccountantGroupMemberSearch : PaginateSearch
    {

        /// <summary>
        /// 群組Id
        /// </summary>        
        [Required]        
        public int AccountantGroupId { get; set; }
    }
}

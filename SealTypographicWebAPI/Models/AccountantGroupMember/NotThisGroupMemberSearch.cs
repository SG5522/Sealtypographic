using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 非該群組的會計師搜尋條件
    /// </summary>
    public class NotThisGroupMemberSearch : PaginateSearch
    {
        /// <summary>
        /// 群組Id
        /// </summary>        
        [Required]        
        public int AccountantGroupId { get; set; }
    }
}

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
        /// 會計師編號或名稱
        /// </summary>
        /// <example>ACC000 or 王XX</example>
        public string? KeyWord { get; set; }

        /// <summary>
        /// 群組Id
        /// </summary>        
        [Required]        
        public int AccountantGroupId { get; set; }
    }
}

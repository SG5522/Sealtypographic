using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師分頁搜尋
    /// </summary>
    public class AccountantSearch : PaginateSearchWithStatus
    {
        /// <summary>
        /// 會計師ID或名字或是群組名稱
        /// </summary>        
        public string? IdOrNameOrGroupsName { get; set; }        
    }
}

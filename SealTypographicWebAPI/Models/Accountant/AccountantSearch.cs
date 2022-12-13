using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師分頁搜尋
    /// </summary>
    public class AccountantSearch : PaginateSearch
    {
        /// <summary>
        /// 會計師編號或名字或是群組名稱
        /// </summary>        
        /// <example>ACC001 or 會計名字 or 會計群組名稱</example>
        public string? NumberOrNameOrGroupsName { get; set; }        
    }
}

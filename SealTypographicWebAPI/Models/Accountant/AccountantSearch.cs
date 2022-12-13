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
        /// 會計師ID或名字或是群組名稱
        /// </summary>        
        public string? IdOrNameOrGroupsName { get; set; }        
    }
}

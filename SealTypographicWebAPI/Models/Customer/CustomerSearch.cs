using SealTypographicWebAPI.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶分頁搜尋
    /// </summary>
    public class CustomerSearch : PaginateSearchWithStatus
    {
        /// <summary>
        /// 搜尋客戶ID或是名字
        /// </summary>
        /// <example>AAA001 or 公司</example>
        public string? CustomerNumberOrName { get; set; }       
    }
}

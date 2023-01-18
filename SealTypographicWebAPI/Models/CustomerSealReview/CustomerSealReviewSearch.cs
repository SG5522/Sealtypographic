using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶分頁搜尋
    /// </summary>
    public class CustomerSealReviewSearch : PaginateSearch
    {
        /// <summary>
        /// 搜尋客戶編號或是名字
        /// </summary>
        /// <example>AAA001 or 公司</example>
        public string? CustomerNumberOrName { get; set; }

        /// <summary>
        /// 審查狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }
    }
}

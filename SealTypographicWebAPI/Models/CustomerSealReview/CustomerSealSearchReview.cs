using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核狀態分頁搜尋
    /// </summary>
    public class CustomerSealSearchReview : PaginateSearch
    {
        /// <summary>
        /// 搜尋客戶編號或是名字
        /// </summary>
        /// <example>AAA001 or 公司</example>
        public string? KeyWord { get; set; }

        /// <summary>
        /// 審查狀態
        /// 請參考 /api/ReviewStatus 的內容
        /// </summary>
        public ReviewStatus? ReviewStatus { get; set; }
    }
}

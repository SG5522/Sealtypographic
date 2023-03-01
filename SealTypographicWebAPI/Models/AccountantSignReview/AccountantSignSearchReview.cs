using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantSignReview
{
    /// <summary>
    /// 會計師簽印審核狀態分頁搜尋
    /// </summary>
    public class AccountantSignSearchReview : PaginateSearch
    {
        /// <summary>
        /// 搜尋會計師編號或是名字
        /// </summary>
        /// <example>AAA001 or 王XX</example>
        public string? KeyWord { get; set; }

        /// <summary>
        /// 審查狀態
        /// 請參考 /api/ReviewStatus 的內容
        /// </summary>
        public ReviewStatus? ReviewStatus { get; set; }
    }
}

using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核
    /// </summary>
    public class CustomerSealReviewViewModel : BaseName
    {
        /// <summary>
        /// 客戶編號
        /// </summary>
        public string CustomerNumber { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        public string BAN { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }
    }

    /// <summary>
    /// 審核列表(分頁)
    /// </summary>
    public class CustomerSealReviewViewModelResponse : PaginateViewModel
    {

        /// <summary>
        /// 審核LIST
        /// </summary>
        public List<CustomerSealReviewViewModel> CustomerSealReviewViewModels { get; set; }
    }
}

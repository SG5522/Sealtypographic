using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核
    /// </summary>
    public class CustomerSealQuarterViewModel : BaseName
    {
        /// <summary>
        /// 客戶編號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public string ReviewStatus { get; set; }
    }

    /// <summary>
    /// 審核列表(分頁)
    /// </summary>
    public class CustomerSealQuarterViewModelResponse : PaginateViewModel
    {

        /// <summary>
        /// 審核LIST
        /// </summary>
        public List<CustomerSealQuarterViewModel> CustomerSealReviewViewModels { get; set; }
    }
}

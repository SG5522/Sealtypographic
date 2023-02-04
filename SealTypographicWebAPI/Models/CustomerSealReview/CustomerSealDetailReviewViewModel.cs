using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核詳細資料
    /// </summary>
    public class CustomerSealDetailReviewViewModel : CustomerDetailData
    {
        /// <summary>
        /// 顯示此筆季度
        /// </summary>         
        /// <example>111Q1</example>
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶印鑑組
        /// </summary>
        public List<CustomerSealViewModel> CustomerSealViewModels { get; set; }
    }

}

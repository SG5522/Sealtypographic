using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities.BaseEntities;
using SealTypographicWebAPI.Models.Customer;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核詳細資料
    /// </summary>
    public class CustomerSealReviewDetail : CustomerDetail
    {
        /// <summary>
        /// 顯示此筆季度
        /// </summary>
        /// <example>111YQ1</example>        
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶印鑑
        /// </summary>
        public List<CustomerSealViewModel> CustomerSealViewModels { get; set; }
    }

    /// <summary>
    /// 客戶印鑑審核詳細資料包含回應訊息
    /// </summary>
    public class CustomerSealReviewDetailResponse : ResponseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomerSealReviewDetailResponse()
        {
            CustomerSealReviewDetail = new();
        }          

        /// <summary>
        /// 客戶印鑑審核詳細資料
        /// </summary>
        public CustomerSealReviewDetail CustomerSealReviewDetail { get; set; }
    }
}

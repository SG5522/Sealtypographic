using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities.BaseEntities;
using SealTypographicWebAPI.Models.Customer;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核
    /// </summary>
    public class CustomerSealReviewDetail : CustomerDetail
    {
        /// <summary>
        /// 印鑑季度
        /// </summary>
        /// <example>111年Q1</example>
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶印鑑
        /// </summary>
        public List<CustomerSealViewModel> CustomerSealViewModels { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomerSealReviewDetailResponse : ResponseViewModel
    {

        /// <summary>
        /// 審核LIST
        /// </summary>
        public CustomerSealReviewDetail CustomerSealReviewDetail { get; set; }
    }
}

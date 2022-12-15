using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核
    /// </summary>
    public class CustomerSealReviewForm 
    {
        /// <summary>
        /// 客戶印鑑ID
        /// </summary>
        public List<int> Ids { get; set; }
    }
}

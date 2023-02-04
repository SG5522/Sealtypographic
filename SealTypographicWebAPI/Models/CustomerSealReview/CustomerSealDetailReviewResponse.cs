namespace SealTypographicWebAPI.Models.CustomerSealReview
{    
    /// <summary>
     /// 客戶印鑑審核詳細資料包含回應訊息
     /// </summary>
    public class CustomerSealDetailReviewResponse : ResponseViewModel
    {
        /// <summary>
        /// new ViewModels
        /// </summary>
        public CustomerSealDetailReviewResponse()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 客戶印鑑審核詳細資料
        /// </summary>
        public CustomerSealDetailReviewViewModel ViewModels { get; set; }
    }
}

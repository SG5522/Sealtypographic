namespace SealTypographicWebAPI.Models.CustomerSealReview
{    
    /// <summary>
     /// 客戶印鑑審核詳細資料包含回應訊息
     /// </summary>
    public class CustomerSealQuarterDetailReviewResponse : ResponseViewModel
    {
        /// <summary>
        /// new ViewModels
        /// </summary>
        public CustomerSealQuarterDetailReviewResponse()
        {
            ViewModel = new();
        }

        /// <summary>
        /// 客戶印鑑審核詳細資料
        /// </summary>
        public CustomerSealQuarterDetailReviewViewModel ViewModel { get; set; }
    }
}

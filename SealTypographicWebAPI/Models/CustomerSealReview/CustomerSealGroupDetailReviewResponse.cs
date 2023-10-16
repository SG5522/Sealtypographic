namespace SealTypographicWebAPI.Models.CustomerSealReview
{    
    /// <summary>
     /// 客戶印鑑審核詳細資料包含回應訊息
     /// </summary>
    public class CustomerSealGroupDetailReviewResponse : ResponseViewModel
    {
        /// <summary>
        /// new ViewModels
        /// </summary>
        public CustomerSealGroupDetailReviewResponse()
        {
            ViewModel = new();
        }

        /// <summary>
        /// 客戶印鑑審核詳細資料
        /// </summary>
        public CustomerSealGroupDetailReviewViewModel ViewModel { get; set; }
    }
}

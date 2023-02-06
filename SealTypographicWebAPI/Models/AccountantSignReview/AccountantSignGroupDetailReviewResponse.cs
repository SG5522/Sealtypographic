namespace SealTypographicWebAPI.Models.AccountantSignReview
{
    /// <summary>
    /// 會計師簽印審核詳細資料包含回應訊息
    /// </summary>
    public class AccountantSignGroupDetailReviewResponse : ResponseViewModel
    {
        /// <summary>
        /// New AccountantSignReviewDetailViewModel
        /// </summary>
        public AccountantSignGroupDetailReviewResponse()
        {
            ViewModel = new();
        }

        /// <summary>
        /// 客戶印鑑審核詳細資料
        /// </summary>
        public AccountantSignGroupDetailReviewViewModel ViewModel { get; set; }
    }
}

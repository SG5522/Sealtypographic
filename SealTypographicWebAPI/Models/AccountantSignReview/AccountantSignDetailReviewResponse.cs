namespace SealTypographicWebAPI.Models.AccountantSignReview
{
    /// <summary>
    /// 會計師簽印審核詳細資料包含回應訊息
    /// </summary>
    public class AccountantSignDetailReviewResponse : ResponseViewModel
    {
        /// <summary>
        /// New AccountantSignReviewDetailViewModel
        /// </summary>
        public AccountantSignDetailReviewResponse()
        {
            ViewModel = new();
        }

        /// <summary>
        /// 客戶印鑑審核詳細資料
        /// </summary>
        public AccountantSignDetailReviewViewModel ViewModel { get; set; }
    }
}

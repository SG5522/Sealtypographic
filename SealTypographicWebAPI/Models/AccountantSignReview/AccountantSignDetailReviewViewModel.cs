using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.CustomerSealReview;

namespace SealTypographicWebAPI.Models.AccountantSignReview
{
    /// <summary>    
    /// 會計師簽印審核詳細資料
    /// </summary>
    public class AccountantSignDetailReviewViewModel : AccountantDetailData
    {
        /// <summary>
        /// 會計師簽印組
        /// </summary>
        public List<AccountantSignViewModel> SignViewModels { get; set; }
    }    
}

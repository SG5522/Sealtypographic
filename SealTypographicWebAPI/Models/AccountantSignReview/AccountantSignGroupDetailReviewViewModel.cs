using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.CustomerSealReview;

namespace SealTypographicWebAPI.Models.AccountantSignReview
{
    /// <summary>    
    /// 會計師簽印審核詳細資料
    /// </summary>
    public class AccountantSignGroupDetailReviewViewModel : AccountantDetailData
    {
        /// <summary>
        /// new Signs
        /// </summary>
        public AccountantSignGroupDetailReviewViewModel() 
        {
            Signs = new();
        }

        /// <summary>
        /// 會計師簽印組Id
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// 會計師簽印組
        /// </summary>
        public List<AccountantSignViewModel> Signs { get; set; }
    }    
}

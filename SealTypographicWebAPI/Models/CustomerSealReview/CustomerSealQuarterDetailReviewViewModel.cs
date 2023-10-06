using SealTypographicWebAPI.Models.CustomerSeal;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核詳細資料
    /// </summary>
    public class CustomerSealQuarterDetailReviewViewModel : CustomerDetailData
    {
        /// <summary>
        /// new Seals
        /// </summary>
        public CustomerSealQuarterDetailReviewViewModel() 
        {
            Seals = new();
        }

        /// <summary>
        /// 印鑑季度Id
        /// </summary>         
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// 顯示此筆季度
        /// </summary>         
        /// <example>111Q1</example>
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶印鑑組
        /// </summary>
        public List<CustomerSealViewModel> Seals { get; set; }
    }

}

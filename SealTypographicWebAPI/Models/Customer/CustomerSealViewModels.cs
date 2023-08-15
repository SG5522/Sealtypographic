using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.Customer
{   
    /// <summary>
    /// 印鑑組
    /// </summary>
    public class CustomerSealViewModels : ResponseViewModel
    {
        /// <summary>
        /// new SealViewModels
        /// </summary>
        public CustomerSealViewModels()
        {
            SealViewModels = new();
        }

        /// <summary>
        /// 客戶印鑑季度ID
        /// </summary>
        /// <example>1</example>
        public int CustomerSealQuarterId { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        /// <example>111Q1</example>
        public string Quarter { get; set; }

        /// <summary>
        /// 審核狀態
        /// 請參考 /api/ReviewStatus 的內容
        /// </summary>
        /// <example>0</example>
        public ReviewStatus ReviewStatus { get; set; }

        /// <summary>
        /// 客戶印鑑組
        /// </summary>
        public List<CustomerSealViewModel> SealViewModels { get; set; }
    }
}

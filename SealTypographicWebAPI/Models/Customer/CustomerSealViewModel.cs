using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 印鑑資料
    /// </summary>
    public class CustomerSealViewModel : BaseSeal
    {
        /// <summary>
        /// 印鑑類別
        /// 請參考 /api/SealMappingConfig?sealType=1 的內容
        /// </summary>
        /// <example>1</example>
        public CustomerSealType SealMappingConfigId { get; set; }

        /// <summary>
        /// 印鑑序號 1為起始
        /// </summary>        
        /// <example>1</example>
        public int Sequence { get; set; }
    }

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
        /// 客戶ID
        /// </summary>
        /// <example>1</example>
        public int CustomerId { get; set; }

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

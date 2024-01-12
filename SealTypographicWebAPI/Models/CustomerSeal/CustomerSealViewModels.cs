using DBEntities.Consts;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.CustomerSeal
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
        public int QuarterYearId { get; set; }

        /// <summary>
        /// 審核狀態
        /// 請參考 /api/ReviewStatus 的內容
        /// </summary>
        /// <example>0</example>
        public ReviewStatus ReviewStatus { get; set; }

        //-----Log Save-----//
        /// <summary>
        /// 客戶Id
        /// </summary>
        [JsonIgnore]
        public int CustomerId { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        [JsonIgnore]
        public string CustomerName { get; set; }

        /// <summary>
        /// 排版類別
        /// </summary>
        [JsonIgnore]
        public TypographyType TypographyType { get; set; }
        //-----Log Save-----//

        /// <summary>
        /// 客戶印鑑組
        /// </summary>
        public List<CustomerSealViewModel> SealViewModels { get; set; }
    }
}

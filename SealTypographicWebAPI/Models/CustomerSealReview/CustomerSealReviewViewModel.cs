using DBEntities.Consts;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核檢視
    /// </summary>
    public class CustomerSealReviewViewModel
    {
        /// <summary>
        /// New SealImageInfos
        /// </summary>
        public CustomerSealReviewViewModel()
        {
            SealImageInfos = new();
        }

        /// <summary>
        /// 客戶印鑑季度Id
        /// </summary>      
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>映像公司</example>
        public string Name { get; set; }

        /// <summary>
        /// 客戶編號
        /// </summary>
        /// <example>CUS123</example>
        public string Code { get; set; }

        /// <summary>
        /// 年季度Id
        /// </summary>
        /// <example>1</example>
        public int QuarterYearId { get; set; }

        /// <summary>
        /// 審核狀態
        /// 請參考 /api/ReviewStatus 的內容
        /// </summary>
        /// <example>0</example>
        public ReviewStatus ReviewStatus { get; set; }

        //============LogSave============//

        /// <summary>
        /// 客戶Id
        /// </summary>
        [JsonIgnore]
        public int CustomerId { get; set; }

        /// <summary>
        /// 排版類別
        /// </summary>
        [JsonIgnore]
        public TypographyType TypographyType { get; set; }

        /// <summary>
        /// 公曆年季度
        /// </summary>
        [JsonIgnore]
        public string GregorainQuarterYear { get; set; }

        /// <summary>
        /// 顯示用年季度(目前使用民國年)
        /// </summary>
        [JsonIgnore]
        public string DisplayQuarterYear { get; set; }

        //============LogSave============//

        /// <summary>
        /// 印鑑組
        /// </summary>        
        public List<SealImageInfo> SealImageInfos { get; set; }
    }    
}

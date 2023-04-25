using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核檢視
    /// </summary>
    public class CustomerSealQuarterReviewViewModel
    {
        /// <summary>
        /// New SealImageInfos
        /// </summary>
        public CustomerSealQuarterReviewViewModel()
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
        /// 季度
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
        /// 印鑑組
        /// </summary>        
        public List<SealImageInfo> SealImageInfos { get; set; }
    }

    /// <summary>
    /// 印鑑資訊
    /// </summary>
    public class SealImageInfo
    {
        /// <summary>
        /// 類別
        /// 請參考 /api/SealMappingConfig?sealType=1 的內容
        /// </summary>
        /// <example>1</example>
        public CustomerSealType SealMappingConfigId { get; set; }

        /// <summary>
        /// 序號
        /// </summary>
        /// <example>1</example>
        public int Sequence { get; set; }

        /// <summary>
        /// 縮圖字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ThumbnailBase64 { get; set; }
    }
}

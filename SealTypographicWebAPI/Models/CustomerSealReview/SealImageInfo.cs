using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 印鑑資訊
    /// </summary>
    public class SealImageInfo : BaseThumbnailSeal
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
    }
}

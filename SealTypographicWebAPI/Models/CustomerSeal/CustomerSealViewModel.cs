using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSeal
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
}

using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.SealMappingConfig
{
    /// <summary>
    /// 取得圖片群組資料以及回應訊息
    /// </summary>
    public class SealMappingConfigResponseList : ResponseViewModel
    {
        /// <summary>
        /// new SealMappingConfigViewModel
        /// </summary>
        public SealMappingConfigResponseList ()
        {
            SealMappingConfigViewModels = new();
        }

        /// <summary>
        /// 印鑑類別 Type(customer、accountant、letterhead)
        /// </summary>
        /// <example>customer</example>
        public string? SealType { get; set; }

        /// <summary>
        /// 圖片群組資料
        /// </summary>
        public List<SealMappingConfigViewModel> SealMappingConfigViewModels { get; set; }
    }
}

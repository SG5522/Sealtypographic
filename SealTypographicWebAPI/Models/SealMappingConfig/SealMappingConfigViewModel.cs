using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.SealMappingConfig
{
    /// <summary>
    ///  (使用印鑑、簽名、LOGO)
    /// </summary>
    public class SealMappingConfigViewModel : BaseData
    {
        /// <summary>
        /// 類別名稱
        /// </summary>
        /// <example></example>
        public string? Name { get; set; }
    }
}

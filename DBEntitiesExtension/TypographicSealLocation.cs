using DBEntitiesExtension.Base;

namespace DBEntitiesExtension
{

    /// <summary>
    /// 各印鑑簽印排版位置
    /// </summary>
    public class TypographicSealLocation : BasePageLocation
    {
        /// <summary>
        /// 排版素材
        /// </summary>
        public TypographyAsset TypographyAsset { get; set; }
    }
}

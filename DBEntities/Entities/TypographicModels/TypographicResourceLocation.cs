using DBEntities.Consts;
using DBEntities.Entities.Base;

namespace DBEntities.Entities.TypographicModels
{

    /// <summary>
    /// 各印鑑簽印排版位置
    /// </summary>
    public class TypographicResourceLocation : BasePageLocation
    {
        /// <summary>
        /// 排版素材
        /// </summary>
        public TypographicResource TypographicResource { get; set; }
    }
}

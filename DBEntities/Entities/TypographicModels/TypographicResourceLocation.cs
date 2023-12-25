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
        /// 角度
        /// </summary>
        public float? Angle { get; set; }

        /// <summary>
        /// 是否差補點(使用OpenCV差補點演算法修圖)
        /// </summary>
        public bool? IsInpaint { get; set; }

        /// <summary>
        /// 印鑑染色
        /// </summary>
        public SealDyeing? SealDyeing { get; set; }

        /// <summary>
        /// 排版素材
        /// </summary>
        public TypographicResource TypographicResource { get; set; }
    }
}

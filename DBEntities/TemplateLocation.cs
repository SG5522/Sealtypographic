using DBEntities.Base;
using DBEntities.Consts;

namespace DBEntities
{
    /// <summary>
    /// 樣板位置
    /// </summary>
    public class TemplateLocation : BaseLocation
    {
        /// <summary>
        /// 印鑑類型
        /// </summary>
        public SealType SealType { get; set; }

        /// <summary>
        /// 印鑑子類別
        /// </summary>
        public SubSealType SubSealType { get; set; }

        /// <summary>
        /// 樣板
        /// </summary>
        public Template Template { get; set; }
    }
}

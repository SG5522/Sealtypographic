using DBEntities.Entities.Base;

namespace DBEntities.Entities.TemplateModels
{
    /// <summary>
    /// 樣板
    /// </summary>
    public class Template : BaseTemplate
    {
        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// 樣板位置
        /// </summary>
        public IList<TemplateLocation> TemplateLocations { get; set; }
    }
}

using DBEntities.Base;

namespace DBEntities
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
        public List<TemplateLocation> TemplateLocations { get; set; }
    }
}

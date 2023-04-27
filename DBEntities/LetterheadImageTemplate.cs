using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 信頭樣板
    /// </summary>
    public class LetterheadImageTemplate : BaseTemplate
    {
        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<LetterheadImageTemplateLocation> LetterheadImageTemplateLocations { get; set; }
    }
}

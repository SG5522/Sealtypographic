using DBEntities.Entities.TypographicModels;

namespace DBEntities.Entities.Base
{
    /// <summary>
    /// 印鑑擺放位置
    /// </summary>
    public abstract class BasePageLocation : BaseLocation
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 排版頁
        /// </summary>
        public TypographicPage TypographicPage { get; set; }
    }
}

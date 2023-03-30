namespace DBEntities.Base
{
    /// <summary>
    /// 印鑑擺放位置
    /// </summary>
    public abstract class PageLocation : Location
    {
        /// <summary>
        /// 排版頁
        /// </summary>
        public TypographicPage TypographicPage { get; set; }
    }
}

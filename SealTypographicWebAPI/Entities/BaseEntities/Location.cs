namespace SealTypographicWebAPI.Entities.BaseEntities
{
    /// <summary>
    /// 印鑑擺放位置
    /// </summary>
    public class Location : BaseData
    {
        /// <summary>
        /// 頂部位置
        /// </summary>
        public float Top { get; set; }

        /// <summary>
        /// 最左邊位置
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        /// 寬
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 排版頁Id
        /// </summary>
        public int TypographicPageId { get; set; }

        /// <summary>
        /// 排版頁
        /// </summary>
        public TypographicPage TypographicPage { get; set; }
    }
}

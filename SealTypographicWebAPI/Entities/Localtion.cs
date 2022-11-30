namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 印鑑圖像ID 與 位置
    /// </summary>
    public class Localtion
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 頂部位置
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// 最左邊位置
        /// </summary>
        public int Left { get; set; }

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

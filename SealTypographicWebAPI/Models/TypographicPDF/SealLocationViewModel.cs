namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 印鑑圖像ID 與 位置
    /// </summary>
    public class SealLocationViewModel
    {
        /// <summary>
        /// 圖片
        /// </summary>
        public string ImageBase64 { get; set; }

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
    }
}

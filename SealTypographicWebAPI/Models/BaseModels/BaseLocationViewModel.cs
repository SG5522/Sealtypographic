namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 位置
    /// </summary>
    public abstract class BaseLocationViewModel
    {
        /// <summary>
        /// 最左邊位置
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        /// 頂部位置
        /// </summary>
        public float Top { get; set; }

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

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 位置
    /// </summary>
    public abstract class BaseLocation
    {
        /// <summary>
        /// 各印鑑、簽印Id
        /// 此處Id為TypographicResource的Id
        /// </summary>
        public int Id { get; set; }

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

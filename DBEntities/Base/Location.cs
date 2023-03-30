namespace DBEntities.Base
{
    /// <summary>
    /// 印鑑擺放位置
    /// </summary>
    public abstract class Location : BaseData
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

    }
}

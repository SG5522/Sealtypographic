using DBEntities.Consts;

namespace DBEntities.Entities.Base
{
    /// <summary>
    /// 各項樣版基本資料
    /// </summary>
    public abstract class BaseTemplate : BaseSettimg
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 樣板疊放方式
        /// </summary>
        public StackMode? StackMode { get; set; }

        /// <summary>
        /// 樣板疊放位移
        /// </summary>
        public int? StackShift { get; set; }

        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string ImageViewFullPath { get; set; }

        /// <summary>
        /// 縮圖路徑
        /// </summary>
        public string ThumbnailFullPath { get; set; }
    }
}

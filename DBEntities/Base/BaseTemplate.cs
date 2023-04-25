using DBEntities.Consts;

namespace DBEntities.Base
{
    /// <summary>
    /// 各項樣版基本資料
    /// </summary>
    public abstract class BaseTemplate : BaseNameData
    {
        /// <summary>
        /// 文件格式
        /// </summary>
        public PageSize PageSize { get; set; }

        /// <summary>
        /// 頁面方向
        /// </summary>
        public PapeOrientation PaperOrientation { get; set; }

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

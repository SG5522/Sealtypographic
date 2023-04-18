using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 各項樣版基本資料
    /// </summary>
    public abstract class BaseTemplate
    {
        /// <summary>
        /// 樣板名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        public PageSize PageSize { get; set; }

        /// <summary>
        /// 頁面方向
        /// </summary>
        public PapeOrientation PaperOrientation { get; set; }

        /// <summary>
        /// 背景圖片
        /// </summary>
        public IFormFile ImageView { get; set; }

        /// <summary>
        /// 背景圖片
        /// </summary>
        public IFormFile Thumbnail { get; set; }

    }
}

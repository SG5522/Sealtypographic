using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 各項樣板基本資料與回應內容
    /// </summary>
    public abstract class BaseTemplateWithResponse : ResponseViewModel
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
    }
}

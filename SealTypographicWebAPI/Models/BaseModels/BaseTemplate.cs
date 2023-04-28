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
        /// <example>預設樣板</example>
        public string Name { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        /// <example>1</example>
        public PageSize PageSize { get; set; }

        /// <summary>
        /// 頁面方向
        /// </summary>
        /// <example>0</example>
        public PapeOrientation PaperOrientation { get; set; }
    }
}

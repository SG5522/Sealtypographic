using DBEntities.Consts;

namespace DBEntities.Base
{
    public abstract class BaseSettimg : BaseData
    {
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

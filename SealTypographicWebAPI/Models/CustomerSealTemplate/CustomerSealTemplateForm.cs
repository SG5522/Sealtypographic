using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealTemplate
{
    /// <summary>
    /// 客戶樣板 (新增更新使用)
    /// </summary>
    public class CustomerSealTemplateForm : BaseTemplate
    {
        /// <summary>
        /// 樣板疊放方式
        /// </summary>
        public StackMode StackMode { get; set; }

        /// <summary>
        /// 樣板疊放位移
        /// </summary>
        public int StackShift { get; set; }
    }
}

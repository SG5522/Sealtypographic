using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealTemplate
{
    /// <summary>
    /// 客戶印鑑樣板 (更新使用)
    /// </summary>
    public class CustomerSealTemplateUpdateForm : BaseTemplateWithFile
    {
        /// <summary>
        /// 樣板Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 樣板疊放方式
        /// </summary>
        /// <example>1</example>
        public StackMode StackMode { get; set; }

        /// <summary>
        /// 樣板疊放位移
        /// </summary>
        /// <example>1</example>
        public int StackShift { get; set; }

        /// <summary>
        /// 刪除樣板位置座標
        /// </summary>                       
        public List<int> DeleteLocationIds { get; set; }

        /// <summary>
        /// 修改樣板位置座標
        /// </summary>                       
        public List<CustomerSealTemplateLocationUpdateForm> LocationUpdateForms { get; set; }

        /// <summary>
        /// 新增樣板位置座標
        /// </summary>                       
        public List<CustomerSealTemplateLocationForm> LocationForms { get; set; }
    }
}

using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealTemplate
{
    /// <summary>
    /// 客户印鑑樣板位置
    /// </summary> 
    [ModelBinder(BinderType = typeof(JsonModelBinder))]
    public class CustomerSealTemplateLocationUpdateForm : BaseLocationViewModel
    {
        /// <summary>
        /// 客戶印鑑樣板位置Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 客戶印鑑類別
        /// </summary>
        public CustomerSealType CustomerSealType { get; set; }
    }

    /// <summary>
    /// 客戶樣板 (新增更新使用)
    /// </summary>
    public class CustomerSealTemplateUpdateForm : BaseTemplate
    {
        /// <summary>
        /// 樣板疊放方式
        /// </summary>
        public StackMode StackMode { get; set; }

        /// <summary>
        /// 樣板疊放位移
        /// </summary>
        public int StackShift { get; set; }

        /// <summary>
        /// 客戶印鑑樣板位置
        /// </summary>                       
        public List<CustomerSealTemplateLocationUpdateForm> CustomerSealTemplateLocationUpdateForms{ get; set; }
    }
}

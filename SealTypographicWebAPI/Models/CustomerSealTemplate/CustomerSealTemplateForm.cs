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
    public class CustomerSealTemplateLocationForm : BaseLocationViewModel
    {
        /// <summary>
        /// 客戶印鑑類別
        /// </summary>
        public CustomerSealType CustomerSealType { get; set; }
    }

    /// <summary>
    /// 客戶樣板 (新增使用)
    /// </summary>
    public class CustomerSealTemplateForm : BaseTemplateWithFile
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
        public List<CustomerSealTemplateLocationForm> CustomerSealTemplateLocationForms { get; set; }
    }
}

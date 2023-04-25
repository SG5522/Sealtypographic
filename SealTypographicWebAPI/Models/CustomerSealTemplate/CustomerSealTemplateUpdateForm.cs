using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealTemplate
{
    /// <summary>
    /// 客戶印鑑樣板座標
    /// </summary> 
    [ModelBinder(BinderType = typeof(JsonModelBinder))]
    public class CustomerSealTemplateLocationUpdateForm : BaseLocationViewModel
    {
        /// <summary>
        /// 樣板座標Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 客戶印鑑類別
        /// </summary>
        public CustomerSealType CustomerSealType { get; set; }
    }

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
        public StackMode StackMode { get; set; }

        /// <summary>
        /// 樣板疊放位移
        /// </summary>
        public int StackShift { get; set; }

        /// <summary>
        /// 樣板位置座標
        /// </summary>                       
        public List<CustomerSealTemplateLocationUpdateForm> LocationUpdateForms{ get; set; }
    }
}

using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantSignTemplate
{
    /// <summary>
    /// 會計師簽印樣板座標
    /// </summary> 
    [ModelBinder(BinderType = typeof(JsonModelBinder))]
    public class AccountantSignTemplateLocationForm : BaseLocationViewModel
    {
        /// <summary>
        /// 會計師簽印類別
        /// </summary>
        public AccountantSignType AccountantSignType { get; set; }
    }

    /// <summary>
    /// 會計師簽印樣板 (新增使用)
    /// </summary>
    public class AccountantSignTemplateForm : BaseTemplateWithFile
    {
        /// <summary>
        /// 會計師簽印樣板座標
        /// </summary>                       
        public List<AccountantSignTemplateLocationForm> AccountantSignTemplateLocationForms { get; set; }
    }
}

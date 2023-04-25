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
    public class AccountantSignTemplateLocationUpdateForm : BaseLocationViewModel
    {
        /// <summary>
        /// 樣板座標Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 會計師簽印類別
        /// </summary>
        public AccountantSignType AccountantSignType { get; set; }
    }

    /// <summary>
    /// 會計師簽印樣板(分頁)
    /// </summary>
    public class AccountantSignTemplateUpdateForm : BaseTemplateWithFile
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
        /// 樣板座標
        /// </summary>                       
        public List<AccountantSignTemplateLocationUpdateForm> LocationUpdateForms{ get; set; }
    }
}

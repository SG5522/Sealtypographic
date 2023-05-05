using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantSignTemplate
{
    /// <summary>
    /// 會計師簽印樣板座標
    /// </summary>     
    public class AccountantSignTemplateLocationUpdateForm : BaseLocationModel
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
}

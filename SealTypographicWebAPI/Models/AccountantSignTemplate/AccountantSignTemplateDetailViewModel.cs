using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantSignTemplate
{
    /// <summary>
    /// 會計師簽印樣板座標
    /// </summary>
    public class AccountantSignTemplateLocationViewModel : BaseLocation
    {
        /// <summary>
        /// 會計師簽印類別
        /// </summary>
        public AccountantSignType AccountantSignType { get; set; }
    }

    /// <summary>
    /// 會計師簽印樣板詳細
    /// </summary>
    public class AccountantSignTemplateDetailViewModel : BaseTemplateWithResponse
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public AccountantSignTemplateDetailViewModel() 
        {
            LocaltionViewModels = new ();
        }

        /// <summary>
        /// 樣板Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 會計師簽印樣板座標
        /// </summary>
        public List<AccountantSignTemplateLocationViewModel> LocaltionViewModels{ get; set; }
    }
}

using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師印鑑簽名
    /// </summary>
    public class AccountantSignViewModel : BaseSeal
    {

    }
    /// <summary>
    /// 會計師印鑑簽名組
    /// </summary>
    public class AccountantSignViewModels : ResponseViewModel
    {
        /// <summary>
        /// 會計師印鑑簽名組
        /// </summary>
        public List<AccountantSignViewModel> SignViewModels { get; set; }
    }
}

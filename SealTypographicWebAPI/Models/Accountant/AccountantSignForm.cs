using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師印鑑簽名
    /// </summary>
    public class AccountantSignForm : BaseCreateSeal
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public int AccountantID { get; set; }
    }
}

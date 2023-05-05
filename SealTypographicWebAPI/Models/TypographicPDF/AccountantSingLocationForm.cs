using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 會計師簽名與印鑑位置
    /// </summary>
    public class AccountantSingLocationForm : BaseLocationModel
    {
        /// <summary>
        /// 會計師簽印ID
        /// </summary>
        public int AccountantSignId { get; set; }
    }
}

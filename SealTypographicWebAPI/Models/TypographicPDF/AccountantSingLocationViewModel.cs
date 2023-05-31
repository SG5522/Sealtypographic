using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 會計師簽印位置與TIPS顯示
    /// </summary>
    public class AccountantSignLocationViewModel : BaseSealLocationViewModel
    {
        /// <summary>
        /// 會計師名稱
        /// </summary>
        /// <example>劉先生</example>
        public string AccountantName { get; set; }

        /// <summary>
        /// 會計師簽印類別
        /// </summary>
        public AccountantSignType AccountantSignType { get; set; }
    }
}

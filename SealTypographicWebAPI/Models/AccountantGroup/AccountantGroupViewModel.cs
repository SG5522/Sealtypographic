using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroup
{
    /// <summary>
    /// 會計師群組
    /// </summary>
    public class AccountantGroupViewModel : BaseName
    {
        /// <summary>
        /// 會計師群組編號
        /// </summary>
        public string AccountantGroupNumber { get; set; }
    }
}

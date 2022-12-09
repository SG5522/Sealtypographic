using SealTypographicWebAPI.Models.PublicModel;

namespace SealTypographicWebAPI.Models.AccountantGroup
{
    /// <summary>
    /// 建立會計師群組
    /// </summary>
    public class AccountantGroupFormUpdate : BaseUpdateNameData
    {
        /// <summary>
        /// 會計師群組編號
        /// </summary>
        public string AccountantGroupNumber { get; set; }
    }
}

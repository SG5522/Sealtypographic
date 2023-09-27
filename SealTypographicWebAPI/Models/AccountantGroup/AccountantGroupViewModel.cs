using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroup
{
    /// <summary>
    /// 會計師群組
    /// </summary>
    public class AccountantGroupViewModel : BaseData
    {
        /// <summary>
        /// 會計師群組編號
        /// </summary>
        public string AccountantGroupNumber { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>台北群組</example>
        public string Name { get; set; }
    }
}

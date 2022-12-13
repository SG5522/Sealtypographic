using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroup
{
    /// <summary>
    /// 建立會計師群組
    /// </summary>
    public class AccountantGroupFormUpdate : BaseUpdateName
    {
        /// <summary>
        /// 會計師群組編號
        /// </summary>
        /// <example>TAP001</example>
        [RegularExpression(@"^[A-Za-z0-9]+$")]
        [Required]
        public string AccountantGroupNumber { get; set; }
    }
}

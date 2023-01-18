using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師印鑑簽名
    /// </summary>
    public class AccountantSignForm : BaseCreateSeal
    {
        /// <summary>      
        /// </summary>
        /// <example>1</example>
        [Required]        
        public AccountantSignType SealMappingConfigId { get; set; }
    }

    /// <summary>
    /// 會計師簽印組
    /// </summary>
    public class AccountantSignForms
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public int AccountantId { get; set; }

        /// <summary>
        /// 會計師印鑑簽名
        /// </summary>
        public List<AccountantSignForm> SignForms { get; set; }
    }
}

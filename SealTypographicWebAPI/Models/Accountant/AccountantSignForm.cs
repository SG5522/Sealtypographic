using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Utils;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師簽印
    /// </summary>
    public class AccountantSign : BaseCreateSeal
    {
        private AccountantSignType sealMappingConfigId;

        /// <summary>      
        /// </summary>
        /// <example>1</example>
        [Required]
        public AccountantSignType SealMappingConfigId
        {
            get => sealMappingConfigId;
            set
            {
                sealMappingConfigId = value;
                if (sealMappingConfigId != AccountantSignType.None)
                {
                    SealType = SealType.Accountant;
                    SubSealType = SealMappingConfigUtil.GetSubSealTypeWithAccountant(sealMappingConfigId);
                }
            }
        }
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
        [Required]
        public List<AccountantSign> SignForms { get; set; }
    }
}

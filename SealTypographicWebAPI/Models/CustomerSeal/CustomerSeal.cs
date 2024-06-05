using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Utils;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.CustomerSeal
{
    /// <summary>
    /// 客戶印鑑
    /// </summary>
    public class CustomerSeal : BaseCreateSeal
    {
        private CustomerSealType sealMappingConfigId;

        private int sequence;

        /// <summary>
        /// 客戶印鑑群組ID 
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管    
        /// 5.其他(客戶)
        /// </summary>
        /// <example>1</example>
        [Required]
        public CustomerSealType SealMappingConfigId
        {
            get => sealMappingConfigId;
            set
            {
                sealMappingConfigId = value;
                if (sealMappingConfigId != CustomerSealType.None)
                {
                    SealType = SealType.Customer;
                    SubSealType = SealMappingConfigUtil.GetSubSealTypeWithCustomer(sealMappingConfigId);
                }
            }
        }

        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>
        [Required]
        [Range(1, 99)]
        public int Sequence 
        {
            get => sequence;
            set
            {
                sequence = value;
                if (sequence != 0)
                {
                    CommonSequence = sequence;
                }
            }
        }
    }
}

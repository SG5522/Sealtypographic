using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師簽印
    /// </summary>
    public class AccountantSignViewModel : BaseSeal
    {
        /// <summary>         
        /// 會計師簽印類別
        /// 請參考 /api/SealMappingConfig?sealType=2 的內容
        /// </summary>        
        /// <example>1</example>
        public AccountantSignType SealMappingConfigId { get; set; }
    }
    /// <summary>
    /// 會計師印鑑簽印組
    /// </summary>
    public class AccountantSignViewModels : ResponseViewModel
    {
        /// <summary>
        /// new SignViewModels
        /// </summary>
        public AccountantSignViewModels()
        {
            SignViewModels = new();
        }
        /// <summary>
        /// 會計師簽印群組ID
        /// </summary>
        public int AccountantSignGroupId { get; set; }

        /// <summary>
        /// 會計簽印群組建立日期
        /// </summary>
        public DateTime GroupCreateDate { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }

        /// <summary>
        /// 會計師印鑑簽名組
        /// </summary>
        public List<AccountantSignViewModel> SignViewModels { get; set; }
    }
}

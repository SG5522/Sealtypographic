using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.ImageRangeSetting
{
    /// <summary>
    /// 客戶印鑑截取設定
    /// </summary>
    public class AccountantSignRangeLocation : BaseLocation
    {
        /// <summary>
        /// 會計師簽印類別
        /// </summary>
        public AccountantSignType AccountantSignType { get; set; }
    }
}

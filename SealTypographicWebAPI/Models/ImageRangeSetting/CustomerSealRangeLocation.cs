using DBEntities.Consts;
using DBEntities.Entities.Base;

namespace SealTypographicWebAPI.Models.ImageRangeSetting
{
    /// <summary>
    /// 客戶印鑑截取範圍設定
    /// </summary>
    public class CustomerSealRangeLocation : BaseLocation
    {
        /// <summary>
        /// Id
        /// </summary>        
        public int Id { get; set; }

        /// <summary>
        /// 客戶印鑑類別
        /// </summary>
        public CustomerSealType CustomerSealType { get; set; }
    }
}

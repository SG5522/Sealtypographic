using DBEntities.Base;
using DBEntities.Consts;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.SealCaptureRange
{
    /// <summary>
    /// 客戶印鑑截取範圍設定
    /// </summary>
    public class CustomerSealCaptureLocation : BaseLocation
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

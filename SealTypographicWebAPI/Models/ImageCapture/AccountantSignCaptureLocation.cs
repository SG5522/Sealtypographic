using DBEntities.Base;
using DBEntities.Consts;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.SealCaptureRange
{
    /// <summary>
    /// 客戶印鑑截取設定
    /// </summary>
    public class AccountantSignCaptureLocation : BaseLocation
    {
        /// <summary>
        /// Id
        /// </summary>        
        public int Id { get; private set; }

        /// <summary>
        /// 會計師簽印類別
        /// </summary>
        public AccountantSignType AccountantSignType { get; set; }
    }
}

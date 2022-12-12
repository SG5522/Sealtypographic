using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.PublicModel;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 印鑑序號是否重複確認用
    /// </summary>
    public class AccountantSignCheck
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public int AccountantId { get; set; }

        /// <summary>
        /// 客戶印鑑群組ID 
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管    
        /// 5.其他(客戶)
        /// </summary>
        public int SealMappingConfigId { get; set; }
    }
}

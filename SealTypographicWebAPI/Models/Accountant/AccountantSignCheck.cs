
using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師簽印是否重複建立確認用
    /// </summary>
    public class AccountantSignCheck
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public int AccountantId { get; set; }

        /// <summary>
        /// 會計師簽印類別 
        /// </summary>
        public AccountantSignType SealMappingConfigId { get; set; }

        /// <summary>
        /// 啟用日期
        /// </summary>
        public DateTime GroupCreateDate { get; set; }
    }
}

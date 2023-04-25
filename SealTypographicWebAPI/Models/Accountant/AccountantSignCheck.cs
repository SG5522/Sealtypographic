
using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師簽印是否重複建立確認用
    /// </summary>
    public class AccountantSignCheck
    {
        /// <summary>
        /// 會計師簽印類別 
        /// </summary>
        public AccountantSignType SealMappingConfigId { get; set; }

        /// <summary>
        /// 會計師簽印建立日期Id
        /// </summary>
        public int AccountantSignCreateDateJournalId { get; set; }

    }
}

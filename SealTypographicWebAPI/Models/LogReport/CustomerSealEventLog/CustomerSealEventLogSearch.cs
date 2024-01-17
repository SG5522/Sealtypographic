using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class CustomerSealEventLogSearch : LogSearchBase
    {
        /// <summary>
        /// 動作類別(紀錄使用)
        /// </summary>
        public ReviewStatus? ReviewStatus { get; set; }

        /// <summary>
        /// UserId or UserName
        /// </summary>
        public string? KeyWord { get; set; }

        /// <summary>
        /// 客戶編號/名稱
        /// </summary>
        public string? CustomerCodeOrName { get; set; }
    }
}

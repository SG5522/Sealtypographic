using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.AccountantSignLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class AccountantSignEventLogViewModel : LogViewModelBase
    {
        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }

        /// <summary>
        /// 會計師編號
        /// </summary>
        public string AccountantCode { get; set; }

        /// <summary>
        /// 會計師姓名
        /// </summary>
        public string AccountantName { get; set; }
    }
}

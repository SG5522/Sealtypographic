using DBEntities.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.LogReport.AccountantSignLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class AccountantSignEventLogViewModel : LogViewModelBase
    {
        /// <summary>
        /// 會計師編號
        /// </summary>
        [Display(Order = -2)]
        public string AccountantCode { get; set; }

        /// <summary>
        /// 會計師姓名
        /// </summary>
        [Display(Order = -1)]
        public string AccountantName { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        [Display(Order = 7)]
        public ReviewStatus ReviewStatus { get; set; }
    }
}

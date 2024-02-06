using DBEntities.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class CustomerSealEventLogViewModel : LogViewModelBase
    {
        /// <summary>
        /// 公司編號
        /// </summary>
        [Display(Order = -2)]
        public string Code { get; set; }

        /// <summary>
        /// 公司姓名
        /// </summary>
        [Display(Order = -1)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 公曆用的季度字串
        /// </summary>
        [Display(Order = 0)]
        public string DisplayQuarterYear { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        [Display(Order = 7)]
        public ReviewStatus ReviewStatus { get; set; }
    }
}

using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.LogReport.AccountantList
{
    /// <summary>
    /// 會計師成員
    /// </summary>
    public class AccountantMemberViewModel
    {
        /// <summary>
        /// 編號
        /// </summary>
        [Display(Order = 0)]
        public string Code { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Order = 1)]
        public string Name { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        [Display(Order = 2)]
        public string GroupName { get; set; }
    }
}

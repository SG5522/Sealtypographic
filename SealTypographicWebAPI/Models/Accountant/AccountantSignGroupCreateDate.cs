using SealTypographicWebAPI.Models.Customer;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師簽印啟用日期搜尋
    /// </summary>
    public class AccountantSignGroupCreateDate
    {
        /// <summary>
        /// 客戶Id
        /// </summary>
        /// <example>1</example>
        [Required]
        public int AccountantId { get; set; }

        /// <summary>
        /// 啟用時間
        /// </summary>
        /// <example>2022/12/31 00:00:00</example>
        [Required]
        public DateTime GroupCreateDate { get; set; }
    }
    /// <summary>
    /// 客戶季度搜尋表
    /// </summary>
    public class AccountantSignGroupCreateDates : ResponseViewModel
    {
        /// <summary>
        /// 客戶季度搜尋表
        /// </summary>
        public List<AccountantSignGroupCreateDate> GroupCreateDates { get; set; }
    }
}

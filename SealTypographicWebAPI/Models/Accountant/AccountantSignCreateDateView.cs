using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Customer;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師簽印群組創建日期
    /// </summary>
    public class AccountantSignCreateDateView
    {
        /// <summary>
        /// 會計師Id
        /// </summary>
        /// <example>1</example>
        [Required]
        public int AccountantId { get; set; }

        /// <summary>
        /// 簽印群組創建日期
        /// </summary>
        /// <example>0001/01/01 00:00:00</example>
        [Required]
        public DateTime GroupCreateDate { get; set; }

        /// <summary>
        /// 審查狀態
        /// </summary>
        [Required]       
        public ReviewStatus ReviewStatus { get; set; }
    }

    /// <summary>
    /// 會計師簽印群組創建日期列表
    /// </summary>
    public class AccountantSignGroupCreateDateViews : ResponseViewModel
    {
        /// <summary>
        /// new GroupCreateDates 
        /// </summary>
        public AccountantSignGroupCreateDateViews ()
        {
            GroupCreateDates = new();
        }

        /// <summary>
        /// 客戶季度搜尋表
        /// </summary>
        public List<AccountantSignCreateDateView> GroupCreateDates { get; set; }
    }
}

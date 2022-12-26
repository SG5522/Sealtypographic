using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師簽印搜尋(依會計師ID與創建群組日期)
    /// </summary>
    public class AccountantSignGroupCreateDateSearch
    {
        /// <summary>
        /// 會計師Id
        /// </summary>
        /// <example>1</example>
        [Required]
        public int AccountantId { get; set; }

        /// <summary>
        /// 啟用時間
        /// </summary>
        /// <example>0001-01-01T00:00:00.000000</example>
        [Required]
        public DateTime GroupCreateDate { get; set; }
    }
}

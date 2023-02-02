
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 異動會計師簽印資料
    /// </summary>
    public class AccountantSignUpdate
    {
        /// <summary>
        /// 會計師Id
        /// </summary>
        public int AccountantId { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime GroupCreateDate { get; set; }

        /// <summary>
        /// 刪除簽印列表(ID)
        /// </summary>
        public List<int> DeleteAccountantSignIds { get; set; }

        /// <summary>
        /// 更新客戶印鑑列表
        /// </summary>
        public List<AccountantSignUpdateForm> UpdateAccountantSigns { get; set; }


        /// <summary>
        /// 更新會計師簽印列表
        /// </summary>
        public List<AccountantSign> CreateAccountantSigns { get; set; }

    }
}

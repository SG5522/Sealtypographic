
namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 更新會計印鑑組資料
    /// </summary>
    public class AccountantSignUpdate
    {
        /// <summary>
        /// 會計師Id
        /// </summary>
        public int AccountantSignId { get; set; }

        /// <summary>
        /// 啟用日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 刪除會計師簽印列表(ID)
        /// </summary>
        public List<int> DeleteAccountantSignIds { get; set; }

        /// <summary>
        /// 更新會計師簽印列表
        /// </summary>
        public List<AccountantSignFormUpdate> UpdateAccountantSigns{ get; set; }

        /// <summary>
        /// 更新會計師簽印列表
        /// </summary>
        public List<AccountantSignForm> CreateAccountantSigns { get; set; }

    }
}

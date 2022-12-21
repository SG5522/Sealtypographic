
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
        /// 更新會計師簽印列表
        /// </summary>
        public List<AccountantSignForm> CreateAccountantSigns { get; set; }

    }
}

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計資料以及回應訊息
    /// </summary>
    public class AccountantResponse : ResponseViewModel
    {
        /// <summary>
        /// 會計基本資料
        /// </summary>
        public AccountantViewModel AccountantViewModel { get; set; }
    }
}

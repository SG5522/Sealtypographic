namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 建立會計師後回傳資料
    /// </summary>
    public class AccountantCreateResponse : ResponseViewModel
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public int AccountantId { get; set; }
    }
}

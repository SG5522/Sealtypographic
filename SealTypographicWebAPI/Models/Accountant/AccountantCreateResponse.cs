namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 建完會計師後回傳ID
    /// </summary>
    public class AccountantCreateResponse : ResponseViewModel
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public int AccountantId { get; set; }
    }
}

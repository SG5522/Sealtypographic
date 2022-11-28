namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師簽名含ID
    /// </summary>
    public class AccountantSignWithId : AccountantPostData    {
        /// <summary>
        /// 簽名ID
        /// </summary>
        public int ID { get; set; }
    }
}

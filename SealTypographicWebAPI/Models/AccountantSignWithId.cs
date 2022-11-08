namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 會計師簽名含ID
    /// </summary>
    public class AccountantSignWithId :AccountantData
    {
        /// <summary>
        /// 簽名ID
        /// </summary>
        public int ID { get; set; }
    }
}

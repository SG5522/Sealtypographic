namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 會計師資料含ID
    /// </summary>
    public class AccountantDataWithId : AccountantData
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public string? ID { get; set; } = null!;
    }
}

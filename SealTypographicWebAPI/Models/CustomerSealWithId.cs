namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 印鑑組資料(含ID)
    /// </summary>
    public class CustomerSealWithId : CustomerSeal
    {
        /// <summary>
        /// 印鑑ID
        /// </summary>
        public int ID { get; set; }
    }
}

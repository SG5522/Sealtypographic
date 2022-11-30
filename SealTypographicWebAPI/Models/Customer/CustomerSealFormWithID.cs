namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 印鑑組資料(含ID)
    /// </summary>
    public class CustomerSealFormWithID : CustomerSealForm
    {
        /// <summary>
        /// 印鑑ID
        /// </summary>
        public int Id { get; set; }
    }
}

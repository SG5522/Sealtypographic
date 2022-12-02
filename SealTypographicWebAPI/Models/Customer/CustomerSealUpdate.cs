namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 印鑑組資料(含ID)
    /// </summary>
    public class CustomerSealUpdate
    {
        /// <summary>
        /// 印鑑ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }
    }
}

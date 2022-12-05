namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師印鑑簽名
    /// </summary>
    public class AccountantSignUpdate
    {
        /// <summary>
        /// 會計師簽名印鑑ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }
    }
}

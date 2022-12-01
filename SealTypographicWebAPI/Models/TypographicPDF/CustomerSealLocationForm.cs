namespace SealTypographicWebAPI.Models.TypographicPDF
{

    /// <summary>
    /// 客戶印鑑排版位置
    /// </summary>
    public class CustomerSealLocationForm : SealLocationForm
    {
        /// <summary>
        /// 客戶印鑑ID
        /// </summary>
        public int CustomerSealJournalId { get; set; }

    }
}

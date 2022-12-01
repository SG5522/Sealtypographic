namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 會計師簽名與印鑑位置
    /// </summary>
    public class AccountantSingLocationForm : SealLocationForm
    {
        /// <summary>
        /// 會計師印鑑簽名ID
        /// </summary>
        public int AccountantSignJournalId { get; set; }

    }
}

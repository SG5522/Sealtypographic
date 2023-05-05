using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{

    /// <summary>
    /// 客戶印鑑排版位置
    /// </summary>
    public class TemporarySealLocationForm : BaseLocationModel
    {
        /// <summary>
        /// 臨時章印鑑ID
        /// </summary>
        public int TemporarySealId { get; set; }
    }
}

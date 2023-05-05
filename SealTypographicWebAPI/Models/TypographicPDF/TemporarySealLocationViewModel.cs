using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 信頭圖片排版位置
    /// </summary>
    public class TemporarySealLocationViewModel : BaseSealLocationViewModel
    {
        /// <summary>
        /// 臨時章印鑑ID
        /// </summary>
        public int TemporarySealId { get; set; }

        /// <summary>
        /// 印鑑序號
        /// </summary>
        public int Sequence { get; set; }
    }
}

using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 信頭圖片排版位置
    /// </summary>
    public class TemporarySealLocationViewModel : BaseSealLocation
    {
        /// <summary>
        /// 印鑑序號
        /// </summary>
        public int Sequence { get; set; }
    }
}

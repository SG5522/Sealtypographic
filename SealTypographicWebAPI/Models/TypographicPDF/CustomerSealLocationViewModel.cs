using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{

    /// <summary>
    /// 客戶印鑑排版位置
    /// </summary>
    public class CustomerSealLocationViewModel : BaseSealLocation
    {
        /// <summary>
        /// 印鑑序號
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 客戶印鑑排版類別
        /// </summary>
        public CustomerSealType CustomerSealType { get; set; }
    }
}

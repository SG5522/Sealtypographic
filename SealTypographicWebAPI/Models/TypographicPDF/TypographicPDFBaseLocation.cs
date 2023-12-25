using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// PDF上印鑑位置與影像編輯參數
    /// </summary>
    public abstract class TypographicPDFBaseLocation : BaseLocation
    {
        /// <summary>
        /// 角度
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// 是否差補點(使用OpenCV差補點演算法修圖)
        /// </summary>
        public bool IsInpaint { get; set; }

        /// <summary>
        /// 印鑑染色
        /// </summary>
        public SealDyeing SealDyeing { get; set; }
    }
}

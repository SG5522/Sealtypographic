using DBEntities.Base;
using DBEntities.Consts;

namespace DBEntities
{
    /// <summary>
    /// 圖片截取範圍設定
    /// </summary>
    public class ImageCaptureLocation : BaseLocation
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 印鑑類型
        /// </summary>
        public SealType SealType { get; set; }

        /// <summary>
        /// 印鑑子類別
        /// </summary>
        public SubSealType SubSealType { get; set; }

        /// <summary>
        /// 樣板
        /// </summary>
        public ImageCaptureSetting ImageCaptureSetting { get; set; }        
    }
}

using DBEntities.Base;
using DBEntities.Consts;

namespace DBEntities
{
    /// <summary>
    /// 圖片截取範圍設定
    /// </summary>
    public class ImageCaptureSetting : BaseSettimg
    {
        /// <summary>
        /// 使用者
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 圖片截取範圍設定
        /// </summary>
        public IList<ImageCaptureLocation> ImageCaptureLocations { get; set; }
    }
}

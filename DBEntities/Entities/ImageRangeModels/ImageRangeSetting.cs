using DBEntities.Entities.Base;

namespace DBEntities.Entities.ImageRangeModels
{
    /// <summary>
    /// 圖片截取範圍設定
    /// </summary>
    public class ImageRangeSetting : BaseSettimg
    {
        /// <summary>
        /// 使用者
        /// </summary>
        //public User? User { get; set; }

        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company? Company { get; set; }

        /// <summary>
        /// 圖片截取範圍設定
        /// </summary>
        public IList<ImageRangeLocation> ImageRangeLocations { get; set; }
    }
}

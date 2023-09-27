using DBEntities.Base;
using DBEntities.Consts;

namespace DBEntities
{
    /// <summary>
    /// 事務所信頭
    /// </summary>
    public class Letterhead : BaseNameData
    {
        /// <summary>
        /// 會計師事務所
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// 信頭圖片啟用狀態
        ///  0.啟用
        /// 10.停用
        /// </summary>
        public LetterheadImageStatus Status { get; set; }

        /// <summary>
        /// 排版素材
        /// </summary>
        public IList<TypographicResource> TypographicResources { get; set; }
    }
}

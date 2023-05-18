using DBEntitiesExtension.Base;

namespace DBEntitiesExtension
{
    /// <summary>
    /// PDF排版資訊
    /// </summary>
    public class TypographicPDF : BaseReviewData
    {
        /// <summary>
        /// 原始檔名
        /// </summary>
        public string OriginFileName { get; set; }

        /// <summary>
        /// PDF路徑
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public Quarter Quarter { get; set; }

        /// <summary>
        /// 客戶
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// 排版頁
        /// </summary>
        public List<TypographicPage> TypographicPages { get; set; }
    }
}

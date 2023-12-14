using DBEntities.Consts;
using DBEntities.Entities.Base;
using DBEntities.Entities.CustomerModels;

namespace DBEntities.Entities.TypographicModels
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
        /// 排版類別
        /// </summary>
        public TypographyType TypographyType { get; set; }

        /// <summary>
        /// 年季度
        /// </summary>
        public QuarterYear QuarterYear { get; set; }

        /// <summary>
        /// 客戶
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// 上傳檔案資料表
        /// </summary>
        public UploadFile UploadFile { get; set; }

        /// <summary>
        /// 排版頁
        /// </summary>
        public IList<TypographicPage> TypographicPages { get; set; }
    }
}

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 信頭圖組
    /// </summary>
    public class LetterheadImage
    {
        /// <summary>
        /// 信頭ID
        /// </summary>
        public int LetterheadID { get; set; }

        /// <summary>
        /// 信頭圖片群組(類別)
        /// 1.LOGO
        /// 2.地址
        /// </summary>
        public int LetterheadImageGroup { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateOnly CreateDate { get; set; }

        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string ImagePath { get; set; } = null!;
    }
}

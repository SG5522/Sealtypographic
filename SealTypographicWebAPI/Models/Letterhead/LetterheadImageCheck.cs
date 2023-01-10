namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖片是否重複建立確認用
    /// </summary>
    public class LetterheadImageCheck
    {
        /// <summary>
        /// 信頭ID
        /// </summary>
        public int LetterheadId { get; set; }

        /// <summary>
        /// 啟用日期
        /// </summary>
        public DateTime GroupCreateDate { get; set; }

        /// <summary>
        /// 印鑑序號
        /// </summary>
        public int Sequence { get; set; }
    }
}

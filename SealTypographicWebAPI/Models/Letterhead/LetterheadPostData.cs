namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭資料
    /// </summary>
    public class LetterheadPostData
    {
        /// <summary>
        /// 信頭ID
        /// </summary>
        /// <example>Lh001</example>
        public string Id { get; set; }

        /// <summary>
        /// 信頭名稱
        /// </summary>
        /// <example>測試信頭</example>
        public string Name { get; set; }

        /// <summary>
        /// 啟用日期
        /// </summary>
        public DateTime AvailableDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 狀態
        /// 0.待審查
        /// 1.已審查
        /// 2.刪除(系統管理員可以看到資料)
        /// </summary>
        /// <example>0</example>
        public int Status { get; set; }
    }
}

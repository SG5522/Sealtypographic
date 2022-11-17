namespace SealTypographicWebAPI.DbModels
{
    /// <summary>
    /// 事務所信頭資料表
    /// </summary>
    public class Letterhead
    {
        /// <summary>
        /// 信頭ID
        /// </summary>        
        public string Id { get; set; } = null!;

        /// <summary>
        /// 信頭名稱
        /// </summary>
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
        public int Status { get; set; }
    }
}

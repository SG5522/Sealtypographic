namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 事務所信頭圖片歷程資料表
    /// </summary>
    public class LetterheadImageJournal
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  信頭圖片編號(排序) 1為起始
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 啟用日(審查通過才有)
        /// </summary>
        public DateTime AvailableDate { get; set; }

        /// <summary>
        /// 啟用結束日期
        /// </summary>
        public DateTime DeadlineDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 狀態
        /// 0.待審查
        /// 1.通過(審核完成)
        /// 2.退件
        /// 3.刪除(系統管理員可以看到資料)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 事務所信頭ID
        /// </summary>
        public string LetterheadId { get; set; } = null!;

        /// <summary>
        /// 事務所信頭資料表
        /// </summary>
        public Letterhead Letterhead { get; set; }

        /// <summary>
        /// 圖片群組ID 
        /// (目前暫定)
        /// 9.信頭        
        /// </summary>
        public int SealMappingConfigId { get; set; }

        /// <summary>
        /// 圖片群組資料表 (使用印鑑、簽名、LOGO)
        /// </summary>
        public SealMappingConfig SealMappingConfig { get; set; }
    }
}

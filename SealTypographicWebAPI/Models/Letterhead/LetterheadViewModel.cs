namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭資料
    /// </summary>
    public class LetterheadViewModel
    {
        /// <summary>
        /// 信頭ID
        /// </summary>
        /// <example></example>
        public string Id { get; set; }

        /// <summary>
        /// 信頭名稱
        /// </summary>
        /// <example></example>
        public string Name { get; set; }

        /// <summary>
        /// 啟用日期
        /// </summary>
        public DateTime AvailableDate { get; set; }

        /// <summary>
        /// 狀態
        /// 0.待審查
        /// 1.已審查
        /// 2.刪除(系統管理員可以看到資料)
        /// </summary>
        /// <example>0</example>
        public int Status { get; set; }
    }
    /// <summary>
    /// /// 信頭資料列表
    /// </summary>
    public class LetterheadViewModels : Response
    {
        /// <summary>
        /// 現在頁數
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每頁資料筆數(不得小於0)
        /// </summary>
        /// <example>5</example>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 信頭資料列表
        /// </summary>
        public List<LetterheadViewModel> ViewModels { get; set; }
    }

    /// <summary>
    /// /// 信頭資料列表
    /// </summary>
    public class LetterheadResponse : Response
    {
        /// <summary>
        /// 信頭資料
        /// </summary>
        public LetterheadViewModel ViewModel { get; set; }
    }


}

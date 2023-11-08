namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 紀錄使用TypographicPDF Service時所讀取與新增的資料
    /// </summary>
    public class LogBase
    {
        /// <summary>
        /// 使用者Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 使用服務
        /// </summary>
        public string UseService { get; set; }

        /// <summary>
        /// 使用函式
        /// </summary>
        public string UseMethod { get; set; }

        /// <summary>
        /// 輸入ID
        /// </summary>
        public string? Id { get; set; }

    }
}

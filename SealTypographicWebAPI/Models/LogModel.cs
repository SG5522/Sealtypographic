namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 紀錄使用TypographicPDF Service時所讀取與新增的資料
    /// </summary>
    public class LogModel
    {
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

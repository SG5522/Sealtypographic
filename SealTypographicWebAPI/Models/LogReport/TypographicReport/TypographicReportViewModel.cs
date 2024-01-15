using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.LogReport.TypographicReport
{
    /// <summary>
    /// 客戶財稅報排版紀錄
    /// </summary>
    public class TypographicReportViewModel
    {
        /// <summary>
        /// 使用者Id
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// 排版的使用者Id
        /// </summary>
        /// <example>admin</example>
        public string UserName { get; set; }

        /// <summary>
        /// 建立排版的使用者名稱
        /// </summary>
        /// <example>管理者</example>
        public string UserNickName { get; set; }

        /// <summary>
        /// 客戶編碼
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 紀錄日期
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string EditFileName { get; set; }

        /// <summary>
        /// 排版頁數
        /// </summary>
        public int EditPageCount { get; set; }

        /// <summary>
        /// 空白頁數
        /// </summary>
        public int? BlankPageCount { get; set; }
    }
}

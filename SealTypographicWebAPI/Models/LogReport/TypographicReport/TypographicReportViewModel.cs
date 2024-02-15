using System.ComponentModel.DataAnnotations;
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
        [Display(Order = 0)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 建立排版的使用者名稱
        /// </summary>
        /// <example>管理者</example>
        [Display(Order = 1)]
        public string UserNickName { get; set; } = string.Empty;

        /// <summary>
        /// 客戶編碼
        /// </summary>
        [Display(Order = 2)]
        public string CustomerCode { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        [Display(Order = 3)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 紀錄日期
        /// </summary>
        [Display(Order = 4)]
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        [Display(Order = 5)]
        public string EditFileName { get; set; }

        /// <summary>
        /// 排版頁數
        /// </summary>
        [Display(Order = 6)]
        public int EditPageCount { get; set; }

        /// <summary>
        /// 空白頁數
        /// </summary>
        [Display(Order = 7)]
        public int? BlankPageCount { get; set; }
    }
}

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 錯誤訊息使用的Class
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// 錯誤狀態
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? Message { get; set; }
    }
}

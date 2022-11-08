namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 錯誤訊息使用的Class
    /// </summary>
    public class Response
    {
        /// <summary>
        /// 狀態號碼
        /// </summary>
        public int ResponseStatus { get; set; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ResponseMessage { get; set; }
    }
}

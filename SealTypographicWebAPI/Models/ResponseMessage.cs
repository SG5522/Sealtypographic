namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// POST PUT 回傳訊息
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// 回傳錯誤項目
        /// </summary>
        public List<ResponseViewModel> ResponseViewModel { get; set; }
    }
}

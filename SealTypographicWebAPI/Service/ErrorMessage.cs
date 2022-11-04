using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Service
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// 取得錯誤訊息
        /// </summary>
        /// <returns></returns>
        public ErrorData Get()
        {
            ErrorData data = new()
            {
                Status = 0,
                Message = "Error"
            };
            return data;
        }
    }
}

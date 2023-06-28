using Microsoft.Extensions.Logging.Abstractions;

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 登入資料
    /// </summary>
    public class LoginData
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        public string UserID { get; set; } = null!;
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = null!;
    }
}

using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public class StatusService
    {
        /// <summary>
        /// 取得Status名稱
        /// </summary>
        /// <returns></returns>
        public string Get(Status status)
        {
            return status switch
            {
                Status.Pending => "待審",
                Status.Approval => "已審核",
                Status.Hidden => "刪除(隱藏)",
                _ => "",
            };
        }
    }
}

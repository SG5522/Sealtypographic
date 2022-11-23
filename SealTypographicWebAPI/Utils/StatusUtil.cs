using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public class StatusUtil
    {
        /// <summary>
        /// 取得Status名稱
        /// </summary>
        /// <returns></returns>
        public static string Get(Status status)
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

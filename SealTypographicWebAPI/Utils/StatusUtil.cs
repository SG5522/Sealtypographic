using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public class StatusUtil
    {        
        /// <summary>
        /// 待審
        /// </summary>
        /// <returns></returns>
        public static string Pending()
        {
            return Get(Status.Pending);
        }

        /// <summary>
        /// 通過(審核完成)
        /// </summary>
        /// <returns></returns>
        public static string Approval()
        {
            return Get(Status.Approval);
        }

        /// <summary>
        /// 退件
        /// </summary>
        /// <returns></returns>
        public static string Reject()
        {
            return Get(Status.Reject);
        }

        /// <summary>
        /// 隱藏(被刪除時的狀態)
        /// </summary>
        /// <returns></returns>
        public static string Hidden()
        {
            return Get(Status.Hidden);
        }

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
                Status.Reject => "退件",
                Status.Hidden => "刪除(隱藏)",
                _ => "",
            };
        }
    }
}

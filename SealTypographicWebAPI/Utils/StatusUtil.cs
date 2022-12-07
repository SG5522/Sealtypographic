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
            return Get(ReviewStatus.Pending);
        }

        /// <summary>
        /// 通過(審核完成)
        /// </summary>
        /// <returns></returns>
        public static string Approval()
        {
            return Get(ReviewStatus.Approval);
        }

        /// <summary>
        /// 退件
        /// </summary>
        /// <returns></returns>
        public static string Reject()
        {
            return Get(ReviewStatus.Reject);
        }

        /// <summary>
        /// 隱藏(被刪除時的狀態)
        /// </summary>
        /// <returns></returns>
        public static string Hidden()
        {
            return Get(ReviewStatus.Hidden);
        }

        /// <summary>
        /// 取得Status名稱
        /// </summary>
        /// <returns></returns>
        public static string Get(ReviewStatus status)
        {
            return status switch
            {
                ReviewStatus.Pending => "待審",
                ReviewStatus.Approval => "已審核",
                ReviewStatus.Reject => "退件",
                ReviewStatus.Hidden => "刪除(隱藏)",
                _ => "",
            };
        }
    }
}

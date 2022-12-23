using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public class ReviewStatusUtil
    {
        /// <summary>
        /// 通過(審核完成)
        /// </summary>
        /// <returns></returns>
        public static string Approval()
        {
            return Get(ReviewStatus.Approval);
        }

        /// <summary>
        /// 啟用(會計師使用)
        /// </summary>
        /// <returns></returns>
        public static string Activated()
        {
            return Get(ReviewStatus.Activated);
        }

        /// <summary>
        /// 停用(會計師使用)
        /// </summary>
        /// <returns></returns>
        public static string NotActivated()
        {
            return Get(ReviewStatus.NotActivated);
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
        /// 草稿
        /// </summary>
        /// <returns></returns>
        public static string Draft()
        {
            return Get(ReviewStatus.Draft);
        }

        /// <summary>
        /// 待審
        /// </summary>
        /// <returns></returns>
        public static string Pending()
        {
            return Get(ReviewStatus.Pending);
        }

        /// <summary>
        /// 作廢
        /// </summary>
        /// <returns></returns>
        public static string Invalid()
        {
            return Get(ReviewStatus.Invalid);
        }

        /// <summary>
        /// 取得Status名稱
        /// </summary>
        /// <returns></returns>
        public static string Get(ReviewStatus status)
        {
            return status switch
            {
                ReviewStatus.Approval => "通過",
                ReviewStatus.Activated => "啟用",
                ReviewStatus.NotActivated => "停用",                                                
                ReviewStatus.Reject => "退件",
                ReviewStatus.Draft => "草稿",
                ReviewStatus.Pending => "待審",
                ReviewStatus.Invalid => "作廢",
                _ => "",
            };
        }
    }
}

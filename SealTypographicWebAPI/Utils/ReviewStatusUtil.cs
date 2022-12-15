using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public class ReviewStatusUtil
    {        
        /// <summary>
        /// 待審
        /// </summary>
        /// <returns></returns>
        public static string Pending()
        {
            return Get(Consts.ReviewStatus.Pending);
        }

        /// <summary>
        /// 通過(審核完成)
        /// </summary>
        /// <returns></returns>
        public static string Approval()
        {
            return Get(Consts.ReviewStatus.Approval);
        }

        /// <summary>
        /// 退件
        /// </summary>
        /// <returns></returns>
        public static string Reject()
        {
            return Get(Consts.ReviewStatus.Reject);
        }


        /// <summary>
        /// 取得Status名稱
        /// </summary>
        /// <returns></returns>
        public static string Get(Consts.ReviewStatus status)
        {
            return status switch
            {
                Consts.ReviewStatus.Pending => "待審",
                Consts.ReviewStatus.Approval => "通過",
                Consts.ReviewStatus.Reject => "退件",                
                _ => "",
            };
        }
    }
}

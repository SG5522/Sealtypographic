using DBEntities.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 啟用日期
    /// </summary>
    public class DateUtil
    {
        /// <summary>
        /// 取得未啟用時間(0000/01/01)
        /// </summary>
        /// <returns></returns>
        public static DateTime NotActivated()
        {
            return Get(Available.NotActivated);
        }

        /// <summary>
        /// 取得啟用時間
        /// </summary>
        /// <returns></returns>
        public static DateTime Activated()
        {
            return Get(Available.Activated);
        }

        private static DateTime Get(Available available)
        {
            switch (available)
            {
                case Available.NotActivated:
                    return DateTime.Parse("0001/01/01");
                case Available.Activated:
                    return DateTime.Now;
                default:
                    return DateTime.Parse("0001/01/01");
            }
        }
    }
}

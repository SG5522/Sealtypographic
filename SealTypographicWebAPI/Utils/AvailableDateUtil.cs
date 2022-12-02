using Microsoft.AspNetCore.SignalR;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 啟用日期
    /// </summary>
    public class AvailableDateUtil
    {
        /// <summary>
        /// 取得未啟用時間(0000/01/01)
        /// </summary>
        /// <returns></returns>
        public static DateTime NotActivated()
        {
            return Get(AvailableDate.NotActivated);
        }

        /// <summary>
        /// 取得啟用時間
        /// </summary>
        /// <returns></returns>
        public static DateTime Activated()
        {
            return Get(AvailableDate.Activated);
        }

        private static DateTime Get(AvailableDate availableDate)
        {
            switch (availableDate)
            {
                case AvailableDate.NotActivated:
                    return DateTime.Parse("0001/01/01");
                case AvailableDate.Activated:
                    return DateTime.Now;
                default:
                    return DateTime.Parse("0001/01/01");
            }
        }
    }
}

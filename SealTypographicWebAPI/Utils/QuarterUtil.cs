using DBEntities;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 取得季度字串
    /// </summary>
    public class QuarterUtil
    {
        /// <summary>
        /// 取得公曆用的季度字串
        /// </summary>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public static string GetGregorainQuarter(QuarterYear quarter)
        {                        
            return $"{quarter.GregorianYear}{quarter.Period}";
        }

        /// <summary>
        /// 取得台灣用的季度字串
        /// </summary>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public static string GetTaiwanYearQuarter(QuarterYear quarter)
        {            
            return $"{quarter.GregorianYear - 1911}{quarter.Period}";
        }
    }
}

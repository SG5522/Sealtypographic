using DBEntities.Entities;

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
        /// <param name="quarterYear"></param>
        /// <returns></returns>
        public static string GetGregorainQuarter(QuarterYear quarterYear)
        {                        
            return $"{quarterYear.GregorianYear}{quarterYear.Period}";
        }

        /// <summary>
        /// 取得台灣用的季度字串
        /// </summary>
        /// <param name="quarterYear"></param>
        /// <returns></returns>
        public static string GetTaiwanYearQuarter(QuarterYear quarterYear)
        {            
            return $"{quarterYear.GregorianYear - 1911}{quarterYear.Period}";
        }

        /// <summary>
        /// 取得台灣用的季度字串
        /// </summary>
        /// <param name="quarterYear"></param>
        /// <returns></returns>
        public static string GetGregorainYear(QuarterYear quarterYear)
        {
            return $"{quarterYear.GregorianYear}";
        }

        /// <summary>
        /// 取得台灣用的季度字串
        /// </summary>
        /// <param name="quarterYear"></param>
        /// <returns></returns>
        public static string GetTaiwanYear(QuarterYear quarterYear)
        {
            return $"{quarterYear.GregorianYear - 1911}";
        }
    }
}

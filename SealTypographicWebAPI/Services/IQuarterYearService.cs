using SealTypographicWebAPI.Models.QuarterYear;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 季度、年度管理
    /// </summary>
    public interface IQuarterYearService
    {
        /// <summary>
        /// 取得季度列表
        /// </summary>
        /// <returns></returns>
        QuarterResponse GetQuarters();

        /// <summary>
        /// 取得年度列表
        /// </summary>
        /// <returns></returns>
        YearResponse GetYears();
    }
}

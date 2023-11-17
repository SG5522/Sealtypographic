using DBEntities.Consts;
using SealTypographicWebAPI.Models.LogReport;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogReportService
    {

        /// <summary>
        /// 取得排版紀錄
        /// </summary>
        /// <param name="customerTypoReportSearch"></param>
        /// <param name="typographyType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        TypographicReportPaginate GetTypographicReport(TypographicReportSearch customerTypoReportSearch, TypographyType typographyType, int userId = 1);
    }
}

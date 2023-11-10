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
        /// <returns></returns>
        CustomerTypoReportPaginate GetCustomerTypoReport(CustomerTypoReportSearch customerTypoReportSearch, TypographyType typographyType, int userId = 0);
    }
}

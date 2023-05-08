using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TypographicPDF;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 排版資訊管理的interface
    /// </summary>
    public interface ITypographicPDFService
    {
        /// <summary>
        /// 取得PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFSearch">搜尋條件</param>
        /// <returns></returns>
        TypographicPDFPaginateViewModel GetPaginate(TypographicPDFSearch typographicPDFSearch);

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFForm">排版資訊(新增使用)</param>
        /// <returns></returns>
        ResponseViewModel New(TypographicPDFForm typographicPDFForm);

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFSaveForm"></param>
        /// <returns></returns>
        ResponseViewModel Save(TypographicPDFSaveForm typographicPDFSaveForm);
    }
}

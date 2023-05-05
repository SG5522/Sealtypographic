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
        TypographicPDFViewModel GetTypographicPDFViewModel(TypographicPDFSearch typographicPDFSearch);

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="pDFId">上傳檔案的pDFId</param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ResponseViewModel New(int pDFId, int customerId);

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFForm"></param>
        /// <returns></returns>
        ResponseViewModel Save(TypographicPDFForm typographicPDFForm);
    }
}

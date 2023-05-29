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
        /// 取得PDF資訊
        /// </summary>
        /// <returns></returns>
        PDFViewModel GetPDFView(int UploadId);

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFForm">排版資訊(新增使用)</param>
        /// <returns></returns>
        TypographicPDFNewResronse New(TypographicPDFForm typographicPDFForm);

        /// <summary>
        /// 儲存PDF排版資訊(更新資料)
        /// </summary>
        /// <param name="typographicPDFSaveForm"></param>
        /// <returns></returns>
        ResponseViewModel Save(TypographicPDFSaveForm typographicPDFSaveForm);

        /// <summary>
        /// 變更PDF排版建檔狀態(未來會變更為審核狀態)
        /// </summary>
        /// <param name="typographicPDFId">PDF排版ID</param>
        /// <returns></returns>
        ResponseViewModel Approval(int typographicPDFId);
    }
}

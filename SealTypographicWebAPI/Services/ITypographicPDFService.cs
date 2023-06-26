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
        /// 取得已編輯PDF頁次資訊
        /// </summary>
        /// <param name="id">TypographicPDFId</param>       
        TypographicPagesResponse GetEditPages(int id);

        /// <summary>
        /// 取得PDF單頁內容
        /// </summary>
        /// <param name="uploadFileid">上傳檔案Id</param>
        /// <param name="pageNumber">pdf頁次</param>  
        /// <returns></returns>
        PDFViewModel GetPDFView(int uploadFileid, int pageNumber);

        /// <summary>
        /// 取得單頁PDF圖像與排版編輯資訊
        /// </summary>
        /// <param name="typographicPDFPageSearch">排板PDFPage搜尋</param>
        /// <returns></returns>
        TypographicPageViewModel GetPageView(TypographicPDFPageSearch typographicPDFPageSearch);

        /// <summary>
        /// 讀取排版PDF的概要
        /// </summary>
        /// <param name="typographicPDFId">PDFID</param>
        /// <returns></returns>
        TypographicPDFSettingViewModel GetTypographicPDFSummary(int typographicPDFId);

        /// <summary>
        /// 建立排版後的PDF
        /// </summary>
        /// <param name="typographicPDFMakeSetting">輸出PDF檔案時的設定</param>
        /// <returns></returns>
        TypographicPDFMakeResponse MakeTyporaphicPDF(TypographicPDFMakeSetting typographicPDFMakeSetting);

        /// <summary>
        /// 取得排版後的PDFBase64
        /// </summary>
        /// <param name="typographicPDFId">PDF排版ID</param>
        /// <returns></returns>
        TypographicPDFEditViewResponse GetEditPDFView(int typographicPDFId);

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

        /// <summary>
        /// 刪除排版PDF(標記刪除)
        /// </summary>
        /// <param name="typographicPDFId"></param>
        /// <returns></returns>
        ResponseViewModel Delete(int typographicPDFId);
    }
}

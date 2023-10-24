using DBEntities.Consts;
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
        /// <param name="typographyType">排版類別</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        TypographicPDFPaginateViewModel GetPaginate(TypographicPDFSearch typographicPDFSearch, TypographyType typographyType, int userId = 0);

        /// <summary>
        /// 取得已編輯PDF頁次資訊
        /// </summary>
        /// <param name="id">TypographicPDFId</param>
        /// <param name="userId">登入的使用者Id</param>       
        TypographicPagesResponse GetEditPages(int id, int userId = 0);

        /// <summary>
        /// 取得PDF單頁內容
        /// </summary>
        /// <param name="uploadFileid">上傳檔案Id</param>
        /// <param name="pageNumber">pdf頁次</param>
        /// <param name="userId">登入的使用者Id</param>  
        /// <returns></returns>
        PDFViewModel GetPDFView(int uploadFileid, int pageNumber, int userId = 0);

        /// <summary>
        /// 取得單頁PDF圖像與排版編輯資訊
        /// </summary>
        /// <param name="typographicPDFPageSearch">排板PDFPage搜尋</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        TypographicPageViewModel GetPageView(TypographicPDFPageSearch typographicPDFPageSearch, int userId = 0);

        /// <summary>
        /// 讀取排版PDF的概要
        /// </summary>
        /// <param name="typographicPDFId">PDFID</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        TypographicPDFSettingViewModel GetTypographicPDFSummary(int typographicPDFId, int userId = 0);

        /// <summary>
        /// 建立排版後的PDF
        /// </summary>
        /// <param name="typographicPDFMakeSetting">輸出PDF檔案時的設定</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        TypographicPDFMakeResponse MakeTyporaphicPDF(TypographicPDFMakeSetting typographicPDFMakeSetting, int userId = 0);

        /// <summary>
        /// 取得排版後的PDFBase64
        /// </summary>
        /// <param name="typographicPDFId">PDF排版ID</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        TypographicPDFEditViewResponse GetEditPDFView(int typographicPDFId, int userId = 0);

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFForm">排版資訊(新增使用)</param>
        /// <param name="typographyType">排版類別</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        TypographicPDFNewResronse New(TypographicPDFForm typographicPDFForm, TypographyType typographyType, int userId = 0);

        /// <summary>
        /// 儲存PDF排版資訊(更新資料)
        /// </summary>
        /// <param name="typographicPDFSaveForm"></param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        ResponseViewModel Save(TypographicPDFSaveForm typographicPDFSaveForm, int userId = 0);

        /// <summary>
        /// 變更PDF排版建檔狀態(未來會變更為審核狀態)
        /// </summary>
        /// <param name="typographicPDFId">PDF排版ID</param>
        /// <param name="reviewStatus">更換審核狀態</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        ResponseViewModel ChangeReviewStatus(int typographicPDFId, ReviewStatus reviewStatus, int userId = 0);

        /// <summary>
        /// 刪除排版PDF(標記刪除)
        /// </summary>
        /// <param name="typographicPDFId"></param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        ResponseViewModel Delete(int typographicPDFId, int userId = 0);
    }
}

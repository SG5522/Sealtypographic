using DBEntities;
using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models.TypographicPDF.EditViewModels;
using SealTypographicWebAPI.Services;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 排版管理(財報)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TypographicPDFController : ControllerBase
    {
        private readonly ITypographicPDFService typographicPDFService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicPDFController(ITypographicPDFService typographicPDFService)
        {
            this.typographicPDFService = typographicPDFService;
        }

        /// <summary>
        /// 取得已編輯PDF頁次資訊
        /// </summary>
        /// <param name="id">TypographicPDFId</param>        
        /// <returns></returns>        
        [HttpGet("{id}")]
        public TypographicPagesResponse EditPages(int id) => typographicPDFService.GetEditPages(id);

        /// <summary>
        /// 取得PDF
        /// </summary>
        /// <param name="uploadId" example="1">上傳檔案Id</param>
        /// <param name="pageNumber" example="1">pdf頁次</param>        
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public PDFViewModel PDFView(int uploadId, int pageNumber) => typographicPDFService.GetPDFView(uploadId, pageNumber);

        /// <summary>
        /// 取得單頁PDF圖像與排版編輯資訊
        /// </summary>
        /// <param name="typographicPDFPageSearch">排板PDFPage搜尋</param> 
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicPageViewModel PageViewModel([FromQuery] TypographicPDFPageSearch typographicPDFPageSearch) => typographicPDFService.GetPageView(typographicPDFPageSearch);

        /// <summary>
        /// 讀取輸出排版PDF的資訊
        /// </summary>
        /// <param name="typographicPDFId">PDFID</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicPDFSettingViewModel PDFSummary(int typographicPDFId) => typographicPDFService.GetTypographicPDFSummary(typographicPDFId);   

        /// <summary>
        /// 排板分頁搜尋
        /// </summary>
        /// <param name="typographicPDFSearch">排版PDF關鍵字搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public TypographicPDFPaginateViewModel Paginate([FromQuery] TypographicPDFSearch typographicPDFSearch) 
            => typographicPDFService.GetPaginate(typographicPDFSearch, TypographyType.FinancialReport);

        /// <summary>
        /// 取得排版後的PDFBase64
        /// </summary>
        /// <param name="typographicPDFId">PDF排版ID</param>
        /// <returns></returns>
        [HttpGet("[Action]/{typographicPDFId}")]
        public TypographicPDFEditViewResponse EditPDFView(int typographicPDFId) => typographicPDFService.GetEditPDFView(typographicPDFId);


        /// <summary>
        /// 新增排版
        /// </summary>
        /// <param name="typographicPDFForm">排版資訊(新增使用)</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TypographicPDFNewResronse> New(TypographicPDFForm typographicPDFForm) => await typographicPDFService.New(typographicPDFForm, TypographyType.FinancialReport);

        /// <summary>
        /// 更新PDF排版
        /// </summary>        
        /// <param name="typographicPDFSaveForm">排板資訊(存檔使用)</param>        
        [HttpPut]
        public async Task<ResponseViewModel> Save(TypographicPDFSaveForm typographicPDFSaveForm) => await typographicPDFService.Save(typographicPDFSaveForm);

        /// <summary>
        /// 建立排版後的PDF(Base64)
        /// </summary>
        /// <param name="typographicPDFMakeSetting">輸出PDF檔案時的設定</param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public TypographicPDFMakeResponse MakePDF(TypographicPDFMakeSetting typographicPDFMakeSetting) => typographicPDFService.MakeTyporaphicPDF(typographicPDFMakeSetting);

        /// <summary>
        /// 變更PDF排版建檔狀態為完成
        /// </summary>        
        /// <param name="typographicPDFId">排板資訊(存檔使用)</param>        
        [HttpPut("{typographicPDFId}")]
        public ResponseViewModel Approval(int typographicPDFId) => typographicPDFService.ChangeReviewStatus(typographicPDFId, ReviewStatus.Approval);

        /// <summary>
        /// 刪除排版PDF(標記刪除)
        /// </summary>
        /// <param name="typographicPDFId"></param>
        [HttpDelete("{typographicPDFId}")]
        public ResponseViewModel Delete(int typographicPDFId) => typographicPDFService.Delete(typographicPDFId);
    }
}

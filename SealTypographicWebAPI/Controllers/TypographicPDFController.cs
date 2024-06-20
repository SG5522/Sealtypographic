using DBEntities.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models.TypographicPDF.EditViewModels;
using SealTypographicWebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 排版管理(財報)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = KeycloakRoleConsts.FINANCIALREPORT_TYPOGRAPHIC)]
    public class TypographicPDFController : APIControllerBase
    {
        private readonly ITypographicPDFService typographicPDFService;

        /// <summary>
        /// 注入Service
        /// </summary>
        public TypographicPDFController(ITypographicPDFService typographicPDFService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.typographicPDFService = typographicPDFService;
        }

        /// <summary>
        /// 取得已編輯PDF頁次資訊
        /// </summary>
        /// <param name="id">TypographicPDFId</param>        
        /// <returns></returns>        
        [HttpGet("{id}")]
        public async Task<TypographicPagesResponse> EditPages(int id) => await typographicPDFService.GetEditPages(id, await GetUserId());

        /// <summary>
        /// 取得PDF
        /// </summary>
        /// <param name="uploadId" example="1">上傳檔案Id</param>
        /// <param name="pageNumber" example="1">pdf頁次</param>
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public async Task<PDFViewModel> PDFView(int uploadId, int pageNumber) 
            => await typographicPDFService.GetPDFView(uploadId, pageNumber, await GetUserId());

        /// <summary>
        /// 取得單頁PDF圖像與排版編輯資訊
        /// </summary>
        /// <param name="typographicPDFPageSearch">排板PDFPage搜尋</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<TypographicPageViewModel> PageViewModel([FromQuery] TypographicPDFPageSearch typographicPDFPageSearch) 
            => await typographicPDFService.GetPageView(typographicPDFPageSearch, await GetUserId());

        /// <summary>
        /// 讀取輸出排版PDF的資訊
        /// </summary>
        /// <param name="typographicPDFId">PDFID</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<TypographicPDFSettingViewModel> PDFSummary(int typographicPDFId) 
            => await typographicPDFService.GetTypographicPDFSummary(typographicPDFId, await GetUserId());   

        /// <summary>
        /// 排板分頁搜尋
        /// </summary>
        /// <param name="typographicPDFSearch">排版PDF關鍵字搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TypographicPDFPaginateViewModel> Paginate([FromQuery] TypographicPDFSearch typographicPDFSearch) 
            => await typographicPDFService.GetPaginate(typographicPDFSearch, TypographyType.FinancialReport, await GetUserId());

        /// <summary>
        /// 取得排版後的PDFBase64
        /// </summary>
        /// <param name="typographicPDFId">PDF排版ID</param>
        /// <returns></returns>
        [HttpGet("[Action]/{typographicPDFId}")]
        public async Task<TypographicPDFEditViewResponse> EditPDFView(int typographicPDFId) 
            => await typographicPDFService.GetEditPDFView(typographicPDFId, await GetUserId());

        /// <summary>
        /// 取得排版步驟
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public PdfEditStepResponse PdfEditStep() => typographicPDFService.GetPdfEditStep();

        /// <summary>
        /// 新增排版
        /// </summary>
        /// <param name="typographicPDFForm">排版資訊(新增使用)</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TypographicPDFNewResronse> New(TypographicPDFForm typographicPDFForm) => await typographicPDFService.New(typographicPDFForm, TypographyType.FinancialReport, await GetUserId());

        /// <summary>
        /// 更新PDF排版
        /// </summary>        
        /// <param name="typographicPDFSaveForm">排板資訊(存檔使用)</param>        
        [HttpPut]
        public async Task<ResponseViewModel> Save(TypographicPDFSaveForm typographicPDFSaveForm) => await typographicPDFService.Save(typographicPDFSaveForm, await GetUserId());

        /// <summary>
        /// 建立排版後的PDF(Base64)
        /// </summary>
        /// <param name="typographicPDFMakeSetting">輸出PDF檔案時的設定</param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public async Task<TypographicPDFMakeResponse> MakePDF(TypographicPDFMakeSetting typographicPDFMakeSetting) 
            => await typographicPDFService.MakeTyporaphicPDF(typographicPDFMakeSetting, await GetUserId());

        /// <summary>
        /// 變更PDF排版建檔狀態為完成
        /// </summary>        
        /// <param name="typographicPDFId">排板資訊(存檔使用)</param>        
        [HttpPut("{typographicPDFId}")]
        public async Task<ResponseViewModel> Approval(int typographicPDFId) => await typographicPDFService.ChangeReviewStatus(typographicPDFId, ReviewStatus.Approval, await GetUserId());

        /// <summary>
        /// 刪除排版PDF(標記刪除)
        /// </summary>
        /// <param name="typographicPDFId"></param>
        [HttpDelete("{typographicPDFId}")]
        public async Task<ResponseViewModel> Delete(int typographicPDFId) => await typographicPDFService.Delete(typographicPDFId, await GetUserId());
    }
}

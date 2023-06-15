using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Services;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 排版管理
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
        public TypographicPagesResponse EditPages(int id)
        {
            TypographicPagesResponse typographicPageResponse = new();
            try
            {
                Log.Information("TypographicPDF PDFView input {@Input}", id);
                typographicPageResponse = typographicPDFService.GetEditPages(id);
                //Log.Information("TypographicPDF PDFView output {@Output}", typographicPageResponse);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF PDFView error {@Error}", ex.Message);
                typographicPageResponse.DbError();
            }
            return typographicPageResponse;
        }

        /// <summary>
        /// 取得PDF
        /// </summary>
        /// <param name="uploadId" example="1">上傳檔案Id</param>
        /// <param name="pageNumber" example="1">pdf頁次</param>        
        /// <returns></returns>        
        [HttpGet("[Action]")]
        public PDFViewModel PDFView(int uploadId, int pageNumber)
        {
            PDFViewModel pDFViewModel = new();
            try
            {
                Log.Information("TypographicPDF PDFView input {@Input}", uploadId, pageNumber);
                pDFViewModel = typographicPDFService.GetPDFView(uploadId, pageNumber);                
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF PDFView error {@Error}", ex.Message);
                pDFViewModel.DbError();
            }            
            return pDFViewModel;
        }

        /// <summary>
        /// 取得單頁PDF圖像與排版編輯資訊
        /// </summary>
        /// <param name="typographicPDFPageSearch">排板PDFPage搜尋</param> 
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicPageViewModel PageViewModel([FromQuery] TypographicPDFPageSearch typographicPDFPageSearch)
        {            
            TypographicPageViewModel typographicPageViewModel = new();
            try
            {
                Log.Information("TypographicPDF PageViewModel input {@Input}", typographicPDFPageSearch);
                typographicPageViewModel = typographicPDFService.GetPageView(typographicPDFPageSearch);
                //Log.Information("TypographicPDF PageViewModel output {@Output}", typographicPageViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF PDFView error {@Error}", ex.Message);
                typographicPageViewModel.DbError();
            }
            return typographicPageViewModel;
        }

        /// <summary>
        /// 讀取輸出排版PDF的資訊
        /// </summary>
        /// <param name="typographicPDFId">PDFID</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicPDFSettingViewModel PDFSummary(int typographicPDFId)
        {
            TypographicPDFSettingViewModel typographicPDFSettingViewModel = new();
            try
            {
                Log.Information("TypographicPDF pdfSummary input {@Input}", typographicPDFId);
                typographicPDFSettingViewModel = typographicPDFService.GetTypographicPDFSummary(typographicPDFId);
                Log.Information("TypographicPDF pdfSummary output {@Output}", typographicPDFSettingViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF pdfSummary error {@Error}", ex.Message);
                typographicPDFSettingViewModel.DbError();
            }
            return typographicPDFSettingViewModel;
        }       

        /// <summary>
        /// 排板分頁搜尋
        /// </summary>
        /// <param name="typographicPDFSearch">排版PDF關鍵字搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public TypographicPDFPaginateViewModel Paginate([FromQuery] TypographicPDFSearch typographicPDFSearch)
        {
            TypographicPDFPaginateViewModel typographicPDFPaginateViewModel = new();
            try
            {
                Log.Information("TypographicPDF paginate input {@Input}", typographicPDFSearch);
                typographicPDFPaginateViewModel = typographicPDFService.GetPaginate(typographicPDFSearch);
                Log.Information("TypographicPDF paginate output {@Output}", typographicPDFPaginateViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF paginate error {@Error}", ex.Message);
                typographicPDFPaginateViewModel.DbError();
            }
            return typographicPDFPaginateViewModel;
        }

        /// <summary>
        /// 新增排版
        /// </summary>
        /// <param name="typographicPDFForm">排版資訊(新增使用)</param>
        /// <returns></returns>
        [HttpPost]
        public TypographicPDFNewResronse New(TypographicPDFForm typographicPDFForm)
        {
            TypographicPDFNewResronse typographicPDFNewResronse = new();
            try
            {
                Log.Information("TypographicPDF new input {@Input}", typographicPDFNewResronse);
                typographicPDFNewResronse = typographicPDFService.New(typographicPDFForm);
                Log.Information("TypographicPDF new output {@Output}", typographicPDFNewResronse);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF New error {@Error}", ex.Message);
                typographicPDFNewResronse.DbError();
            }
            return typographicPDFNewResronse;
        }

        /// <summary>
        /// 更新PDF排版
        /// </summary>        
        /// <param name="typographicPDFSaveForm">排板資訊(存檔使用)</param>        
        [HttpPut]
        public ResponseViewModel Save(TypographicPDFSaveForm typographicPDFSaveForm)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("TypographicPDF save input {@Input}", typographicPDFSaveForm);
                response = typographicPDFService.Save(typographicPDFSaveForm);
                Log.Information("TypographicPDF save output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF save error {@Error}", ex.Message);
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 建立排版後的PDF(Base64)
        /// </summary>
        /// <param name="typographicPDFMakeSetting">輸出PDF檔案時的設定</param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public TypographicPDFMakeResponse MakePDF(TypographicPDFMakeSetting typographicPDFMakeSetting)
        {
            TypographicPDFMakeResponse typographicPagePDFResponse = new();
            try
            {
                Log.Information("TypographicPDF makePDF input {@Input}", typographicPDFMakeSetting);
                typographicPagePDFResponse = typographicPDFService.MakeTyporaphicPDF(typographicPDFMakeSetting);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF MakePDF error {@Error}", ex.Message);
                typographicPagePDFResponse.DbError();
            }
            return typographicPagePDFResponse;
        }

        /// <summary>
        /// 變更PDF排版建檔狀態為完成
        /// </summary>        
        /// <param name="typographicPDFId">排板資訊(存檔使用)</param>        
        [HttpPut("{typographicPDFId}")]
        public ResponseViewModel Approval(int typographicPDFId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("TypographicPDF save input {@Input}", typographicPDFId);
                response = typographicPDFService.Approval(typographicPDFId);
                Log.Information("TypographicPDF save output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF save error {@Error}", ex.Message);
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public ResponseViewModel Delete(int id)
        {
            ResponseViewModel response = new();
            return response;
        }
    }
}

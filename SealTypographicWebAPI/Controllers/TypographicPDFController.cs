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
            return typographicPageResponse;
        }

        /// <summary>
        /// 取得單頁PDF圖像與排版編輯資訊
        /// </summary>
        /// <param name="typographicPDFPageSearch">排板PDFPage搜尋</param> 
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicPageViewModel PageViewModel(TypographicPDFPageSearch typographicPDFPageSearch) 
        {
            TypographicPageViewModel typographicPageViewModel = new();
            return typographicPageViewModel;
        }


        /// <summary>
        /// 排板分頁搜尋
        /// </summary>
        /// <param name="typographicPDFSearch">排版PDF關鍵字搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public TypographicPDFPaginateViewModel Paginate([FromQuery] TypographicPDFSearch typographicPDFSearch)
        {
            TypographicPDFPaginateViewModel typographicPDFPaginateViewModel = new ();
            return typographicPDFPaginateViewModel;
        }

        /// <summary>
        /// 排版資訊(新增使用)
        /// </summary>
        /// <param name="typographicPDFForm"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseViewModel New(TypographicPDFForm typographicPDFForm)
        {
            ResponseViewModel response = new();                            
            try
            {
                response = typographicPDFService.New(typographicPDFForm);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF New error {@Error}", ex.Message);
                response.Error();
            }                
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

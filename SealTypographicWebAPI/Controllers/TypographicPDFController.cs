using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TemplateConfig;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
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
        /// 取得排板PDF Page
        /// </summary>
        /// <param name="typographicPDFPageSearch">// 排板PDF Page搜尋</param>
        /// <returns></returns>
        //[HttpGet("[Action]/{id}")]
        [HttpGet("[Action]")]
        public TypographicPageViewModel GetEditPDFPageView([FromQuery] TypographicPDFPageSearch typographicPDFPageSearch)
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
        /// 新增排板PDF
        /// </summary>
        /// <param name="pDFId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseViewModel New(int pDFId, int customerId)
        {
            ResponseViewModel response = new();                            
            try
            {
                response = typographicPDFService.New(pDFId, customerId);
            }
            catch (Exception ex)
            {
                Log.Error("TypographicPDF New error {@Error}", ex.InnerException);
                response.Error();
            }                
            return response;
        }

        // PUT api/<TypographicPDFController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TypographicPDFController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

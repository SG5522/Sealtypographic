using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.LogReport;
using SealTypographicWebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    ///  
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogReportService logReportService;

        /// <summary>
        /// 建置
        /// </summary>
        public ReportController(ILogReportService logReportService) 
        {
            this.logReportService = logReportService;
        }


        /// <summary>
        /// 取得財報排版紀錄
        /// </summary>
        /// <param name="customerTypoReportSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicReportPaginate FinancialReport([FromQuery] TypographicReportSearch customerTypoReportSearch)
            => logReportService.GetTypographicReport(customerTypoReportSearch, TypographyType.FinancialReport);

        /// <summary>
        /// 取得稅報排版紀錄
        /// </summary>
        /// <param name="customerTypoReportSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicReportPaginate TaxReport([FromQuery] TypographicReportSearch customerTypoReportSearch)
            => logReportService.GetTypographicReport(customerTypoReportSearch, TypographyType.TaxReport);

        // GET api/<ReportController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReportController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReportController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReportController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.TypographicPDF;

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
        // GET: api/<TypographicPDFController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TypographicPDFController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 排板分頁搜尋
        /// </summary>
        /// <param name="typographicPDFSearch">排版PDF搜尋</param>
        /// <returns></returns>
        [HttpGet]
        public TypographicPDFPaginateViewModel Paginate(TypographicPDFSearch typographicPDFSearch)
        {
            TypographicPDFPaginateViewModel typographicPDFPaginateViewModel = new ();
            return typographicPDFPaginateViewModel;
        }

        // POST api/<TypographicPDFController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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

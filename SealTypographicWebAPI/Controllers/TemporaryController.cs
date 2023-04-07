using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 取得臨時章
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TemporaryController : ControllerBase
    {
        /// <summary>
        /// 取得臨時章列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 取得臨時章印鑑組
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<TemporaryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TemporaryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TemporaryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TemporaryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

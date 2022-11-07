using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 會計師基本資料
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AccountantDataController : ControllerBase
    {
        // GET: api/<AccountantDataController>
        [HttpGet]
        public IEnumerable<string> Get(string customerID,string name)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountantDataController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountantDataController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AccountantDataController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountantDataController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

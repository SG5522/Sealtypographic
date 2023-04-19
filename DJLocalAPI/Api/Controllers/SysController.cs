using Microsoft.AspNetCore.Mvc;

namespace DJLocalAPI.Api.Controllers
{
    /// <summary>
    /// 系統功能
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SysController : ControllerBase
    {
        /// <summary>
        /// 簡易測試系統運行狀態
        /// </summary>
        /// <returns></returns>
        // GET: api/<SysController>
        [HttpGet]
        public string Hello()
        {
            return string.Format("Server Run OK. V{0}", typeof(SysController).Assembly.GetName().Version);
        }
    }
}

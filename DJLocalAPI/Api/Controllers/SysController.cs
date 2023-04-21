using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DJLocalAPI.Api.Controllers
{
    /// <summary>
    /// 系統功能
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SysController : ControllerBase
    {
        private readonly ILogger<SysController> logger;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="logger"></param>
        public SysController(ILogger<SysController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 簡易測試系統運行狀態
        /// </summary>
        /// <returns></returns>
        // GET: api/<SysController>
        [HttpGet]
        public string Hello()
        {
            string result = string.Format("Server Run OK. V{0}", typeof(SysController).Assembly.GetName().Version);
            logger.LogInformation(result);
            return result;
        }
    }
}

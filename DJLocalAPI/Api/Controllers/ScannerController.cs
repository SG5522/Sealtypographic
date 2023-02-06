using DJScannerLib.Services;
using Microsoft.AspNetCore.Mvc;
using ScannerLib.Services;

namespace DJLocalAPI.Api.Controllers
{
    /// <summary>
    /// 掃描器功能
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ScannerController : ControllerBase
    {
        /// <summary>
        /// 掃描器元件Service
        /// </summary>
        private readonly IScannerService scannerService; 

        /// <summary>
        /// 建構：注入Service
        /// </summary>
        /// <param name="scannerService"></param>
        public ScannerController(IScannerService scannerService)
        {
            this.scannerService = scannerService;
        }

        /// <summary>
        /// 取得所有驅動清單
        /// </summary>
        /// <returns>回傳清單List</returns>
        [HttpGet("[Action]")]
        public List<string> GetAllDrivers()
        {
            return scannerService.GetAllDrivers();
        }

        /// <summary>
        /// 設置選擇的掃描器
        /// </summary>
        /// <param name="driver">掃描器名稱</param>
        /// <returns></returns>
        [HttpGet]
        public bool SetSelectDriver(string driver)
        {
            return scannerService.SetSelectDriver(driver);
        }
        //public void Scan()
        //{
        //    return scannerService.Scan();
        //}
    }
}

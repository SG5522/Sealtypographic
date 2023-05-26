using DJScannerLib.Models;
using DJScannerLib.Services;
using Microsoft.AspNetCore.Mvc;

namespace DJLocalApp.Api.Controllers
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
        /// 重新取得清單
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult Refresh()
        {
            scannerService.RefreshDrivers();
            return Ok();
        }

        /// <summary>
        /// 取得目前掃描器
        /// </summary>
        /// <returns>掃描器名稱</returns>
        [HttpGet("[Action]")]
        public CurrentDriverResult CurrentDriver()
        {
            return scannerService.GetCurrentDriver();
        }

        /// <summary>
        /// 取得所有掃描器清單
        /// </summary>
        /// <returns>回傳清單List</returns>
        [HttpGet("[Action]")]
        public AllDriversResult AllDrivers()
        {
            return scannerService.GetAllDrivers();
        }

        /// <summary>
        /// 設置掃描器
        /// </summary>
        /// <param name="driver">掃描器名稱</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public bool SetDriver(string driver)
        {
            return scannerService.SetDriver(driver);
        }

        /// <summary>
        /// 執行掃描
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult Scan()
        {
            scannerService.Scan();

            return Ok();
        }

        /// <summary>
        /// 停止掃描
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult StopScan()
        {
            scannerService.StopScan();

            return Ok();
        }


        /// <summary>
        /// 設定掃描
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult Setup()
        {
            scannerService.Setup();

            return Ok();
        }
    }
}

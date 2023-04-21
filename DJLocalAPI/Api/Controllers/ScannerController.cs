using DJScannerLib.Models;
using DJScannerLib.Services;
using Microsoft.AspNetCore.Mvc;

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
        /// 取得預設掃描器
        /// </summary>
        /// <returns>回傳清單List</returns>
        [HttpGet("[Action]")]
        public DefaultDriverResult GetDefaultDriver()
        {
            return scannerService.GetDefaultDriver();
        }

        /// <summary>
        /// 取得所有掃描器清單
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public GetDriversResult GetAllDrivers()
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

        //public void Scan()
        //{
        //    return scannerService.Scan();
        //}
    }
}

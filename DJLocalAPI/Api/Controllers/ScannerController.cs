using Microsoft.AspNetCore.Mvc;
using ScannerLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly ScannerService scannerService; 

        /// <summary>
        /// 建構：注入Service
        /// </summary>
        /// <param name="scannerService"></param>
        public ScannerController(ScannerService scannerService)
        {
            this.scannerService = scannerService;
        }

        /// <summary>
        /// 取得所有驅動清單
        /// </summary>
        /// <returns>回傳清單List</returns>
        [HttpGet]
        public List<string> GetAllDrivers()
        {
            return scannerService.GetAllDrivers();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJScannerLib.Services
{
    public interface IScannerService
    {
        /// <summary>
        /// 取得掃描器清單
        /// </summary>
        /// <returns></returns>
        List<string> GetAllDrivers();

        /// <summary>
        /// 設置選擇的掃描器
        /// </summary>
        /// <param name="driver">掃描器名稱</param>
        /// <returns></returns>
        bool SetSelectDriver(string driver);

        /// <summary>
        /// 掃描
        /// </summary>
        void Scan();
    }
}

using DJScannerLib.Models;

namespace DJScannerLib.Services
{
    /// <summary>
    /// 掃描器功能Service
    /// </summary>
    public interface IScannerService
    {
        /// <summary>
        /// 重新取得所有驅動
        /// </summary>
        /// <returns></returns>
        void RefreshDrivers();

        /// <summary>
        /// 取得目前掃描器
        /// </summary>
        /// <returns></returns>
        CurrentDriverResult GetCurrentDriver();

        /// <summary>
        /// 取得掃描器清單
        /// </summary>
        /// <returns></returns>
        AllDriversResult GetAllDrivers();

        /// <summary>
        /// 設置掃描器
        /// </summary>
        /// <param name="driverName">掃描器名稱</param>
        /// <returns></returns>
        bool SetDriver(string driverName);

        /// <summary>
        /// 執行掃描
        /// </summary>
        void Scan();

        /// <summary>
        /// 停止掃描
        /// </summary>
        void StopScan();

        /// <summary>
        /// 設定掃描器
        /// </summary>
        void Setup();
    }
}

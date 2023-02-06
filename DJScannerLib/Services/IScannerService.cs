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
        /// 初始化Twain
        /// </summary>
        /// <param name="intPtrHwnd"></param>
        void InitTwain(IntPtr intPtrHwnd);
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

        /// <summary>
        ///  Monitor for DG_CONTROL / DAT_NULL / MSG_* stuff (ex MSG_XFERREADY), this
        /// function is only triggered when SetMessageFilter() is called with 'true'...
        /// </summary>
        /// <param name="intPtrHwnd"></param>
        /// <param name="iMsg"></param>
        /// <param name="intPtrWparam"></param>
        /// <param name="intPtrLparam"></param>
        /// <returns></returns>
        bool PreFilterMessage(IntPtr intPtrHwnd, int iMsg, IntPtr intPtrWparam, IntPtr intPtrLparam);
    }
}

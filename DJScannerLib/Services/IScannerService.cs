using DJScannerLib.Models;
using TWAINWorkingGroup;

namespace DJScannerLib.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScannerService
    {
        /// <summary>
        /// 取得TWAIN
        /// </summary>
        /// <returns></returns>
        TWAIN GetTWAIN();

        /// <summary>
        /// 取得預設掃描器
        /// </summary>
        /// <returns></returns>
        DefaultDriverResult GetDefaultDriver();

        /// <summary>
        /// 取得掃描器清單
        /// </summary>
        /// <returns></returns>
        GetDriversResult GetAllDrivers();

        /// <summary>
        /// 設置選擇的掃描器
        /// </summary>
        /// <param name="driver">掃描器名稱</param>
        /// <returns></returns>
        bool SelectedDriver(string driver);

        /// <summary>
        /// 掃描
        /// </summary>
        void Scan();

        /// <summary>
        /// 掃描的callback.  直接調用支持的TWAIN object
        /// This way we don't have to maintain some kind of a loop
        /// inside of the application, which is the source of most problems that
        /// developers run into.
        /// While it looks scary at first, there's really not a lot going on in
        /// here.  We do some sanity checks, we watch for certain kinds of events,
        /// we support the four methods of transferring images, and we dump out
        /// some meta-data about the transferred image.  However, because it does
        /// look scary I dropped in some region pragmas to break things up...
        /// </summary>
        /// <param name="a_blClosing">We're shutting down</param>
        /// <returns>TWAIN status</returns>
        TWAIN.STS ScanCallbackTrigger(bool a_blClosing);

        /// <summary>
        /// Our event handler for the scan callback event.  This will be
        /// called once by ScanCallbackTrigger on receipt of an event
        /// like MSG_XFERREADY, and then will be reissued on every call
        /// into ScanCallback until we're done and get back to state 4.
        ///  
        /// This helps to make sure we're always running in the context
        /// of FormMain on Windows, which is critical if we want drivers
        /// to work properly.  It also gives a way to break up the calls
        /// so the message pump is still reponsive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ScanCallbackEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Rollback the TWAIN state to whatever is requested...
        /// </summary>
        /// <param name="a_state"></param>
        void Rollback(TWAIN.STATE a_state);
    }
}
